#!/bin/bash

set -e
run_cmd="dotnet OzonEdu.MerchandiseApi.dll --no-build -v d"

dotnet OzonEdu.MerchandiseApi.Migrator.dll --no-build -v d -- --dryrun

dotnet OzonEdu.MerchandiseApi.Migrator.dll --no-build -v d

>&2 echo "MerchandiseApiMerchandiseApi DB Migrations complete, starting app."
>&2 echo "Run MerchandiseApi: $run_cmd"
exec $run_cmd