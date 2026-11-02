#!/usr/bin/env bash
# 実機の Pleasanter（Docker・公式イメージ・PostgreSQL）に対する契約テストを、起動から後片付けまで行う（Issue #68）。
#
#   bash tests/contract/run.sh [--keep] [-- <dotnet test の追加の引数>]
#
# 環境変数（すべて省略可）
#   CONTRACT_PROJECT          compose のプロジェクト名。**既定は dotnetclient-contract。** 後片付けはこの名前のものだけに行う
#   CONTRACT_PLEASANTER_PORT  Pleasanter の公開ポート（127.0.0.1 のみ。既定 18095）
#   CONTRACT_PLEASANTER_TAG   implem/pleasanter のタグ（既定は compose.yaml のもの）
#   CONTRACT_SKIP_DOCKER=1    Docker を扱わず、PLEASANTER_BASE_URL・PLEASANTER_API_KEY で渡された既存の Pleasanter に当てる（Actions のサービスコンテナ用）
#
# DB のパスワードと API キーは、実行のたびに乱数で作る。画面にもログにも出さない（--keep のときも出さない）。
set -euo pipefail

here="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
root="$(cd "$here/../.." && pwd)"
keep=0
extra=()
while [ $# -gt 0 ]; do
  case "$1" in
    --keep) keep=1; shift ;;
    --) shift; extra=("$@"); break ;;
    *) echo "不明な引数: $1" >&2; exit 2 ;;
  esac
done

# Windows の Git Bash では、/opt/... などのパスが Windows のパスへ変換されるのを止める
export MSYS_NO_PATHCONV=1

run_tests() {
  local project="$root/tests/VehicleVision.PleasanterTools.DotNetClient.ContractTests"
  if command -v cygpath >/dev/null 2>&1; then
    project="$(cygpath -m "$project")"
  fi
  PLEASANTER_CONTRACT_TESTS=1 dotnet test "$project" \
    -c Release --filter "Category=Contract" "${extra[@]}"
}

if [ "${CONTRACT_SKIP_DOCKER:-0}" = "1" ]; then
  : "${PLEASANTER_BASE_URL:?PLEASANTER_BASE_URL が要る}"
  : "${PLEASANTER_API_KEY:?PLEASANTER_API_KEY が要る}"
  run_tests
  exit $?
fi

export CONTRACT_PROJECT="${CONTRACT_PROJECT:-dotnetclient-contract}"
export CONTRACT_PLEASANTER_PORT="${CONTRACT_PLEASANTER_PORT:-18095}"
export CONTRACT_DB_PASSWORD="$(openssl rand -hex 24)"
api_key="$(openssl rand -hex 64)"
# Windows の docker はネイティブのパスを要る（MSYS_NO_PATHCONV で自動変換を止めているため、ここで直す）
compose_file="$here/compose.yaml"
if command -v cygpath >/dev/null 2>&1; then
  compose_file="$(cygpath -m "$compose_file")"
fi
compose=(docker compose -f "$compose_file" -p "$CONTRACT_PROJECT")

# 他の環境とポートが衝突すると Pleasanter が起動せず、Administrator も作られない
if docker ps --format '{{.Ports}}' | grep -q "127.0.0.1:${CONTRACT_PLEASANTER_PORT}->"; then
  echo "ポート ${CONTRACT_PLEASANTER_PORT} は使用中。CONTRACT_PLEASANTER_PORT で別の値を指定する" >&2
  exit 1
fi

cleanup() {
  if [ "$keep" = "1" ]; then
    echo "--keep: 後片付けをしない。止めるとき: docker compose -f tests/contract/compose.yaml -p $CONTRACT_PROJECT down -v（パスワードを要するので、CONTRACT_DB_PASSWORD を何か設定して実行する）"
    return
  fi
  # このプロジェクト（自分専用の名前）のものだけを止める
  "${compose[@]}" down -v --remove-orphans >/dev/null 2>&1 || true
}
trap cleanup EXIT

echo "起動する（初回はイメージの取得で数分）"
"${compose[@]}" up -d --wait pleasanter

# CodeDefiner は失敗しても終了コード 0 を返すので、ログで確かめる（全文検索が無い旨の 1 件は実害が無い）
if ! "${compose[@]}" logs codedefiner 2>&1 | grep -qi "success"; then
  "${compose[@]}" logs codedefiner 2>&1 | tail -30 >&2
  echo "CodeDefiner が成功していない" >&2
  exit 1
fi

# 試験用の API キーを Administrator へ入れる（Pleasanter は Users.ApiKey をそのまま比べる）。乱数の値はこの実行だけのもの
# SQL に API キーが入るので、失敗時の表示（psql は SQL を echo する）は捨てる
if ! "${compose[@]}" exec -T db psql -U postgres -d Implem.Pleasanter -At -c   "UPDATE \"Implem.Pleasanter\".\"Users\" SET \"ApiKey\"='${api_key}' WHERE \"LoginId\"='Administrator';" 2>/dev/null | grep -q '^UPDATE 1$'; then
  echo "API キーを入れられない（Administrator が無い: CodeDefiner の失敗かポート衝突）" >&2
  exit 1
fi

# 利用者はキャッシュされるので、再起動する。再起動の後に up --wait をやり直さない（CodeDefiner が走り直して DB 接続を巻き込む）
"${compose[@]}" restart pleasanter >/dev/null
base="http://127.0.0.1:${CONTRACT_PLEASANTER_PORT}"
echo "暖機を待つ（/users/login が 200 を返すまで。最大 5 分）"
ok=0
for _ in $(seq 1 100); do
  code="$(curl -s -o /dev/null -w '%{http_code}' "$base/users/login" || true)"
  if [ "$code" = "200" ]; then ok=1; break; fi
  sleep 3
done
if [ "$ok" != "1" ]; then
  "${compose[@]}" logs --tail 40 pleasanter >&2
  echo "Pleasanter が応答しない" >&2
  exit 1
fi

export PLEASANTER_BASE_URL="$base"
export PLEASANTER_API_KEY="$api_key"
run_tests
