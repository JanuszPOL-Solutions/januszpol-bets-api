#!/bin/bash
set -eu

echo "executing as: `whoami` on `hostname`"
echo "system space usage"
df -h --total

TAG=$1
SQL_PASSWORD=$2

DOCKER_IMAGE_PATH="/home/admin/docker-images/januszpol-bets-api-${TAG}.tar.gz"

DB_CONTAINER="januszpol-db-1"
SQLCMD="/opt/mssql-tools/bin/sqlcmd"
d=`date +%d-%m-%Y_%H%M%S`
SQL_DATA_DIR="/var/opt/mssql/data"
BACKUP="$SQL_DATA_DIR/Bets_$d.bak"
SQL_BACKUP_SCRIPT="BACKUP DATABASE Bets TO DISK = '$BACKUP'"
VM_BACKUP_DIR="/home/admin/database_backups"
# backup database
echo "creating database backup: $BACKUP"
docker exec $DB_CONTAINER $SQLCMD -d master -U SA -P $SQL_PASSWORD -Q "$SQL_BACKUP_SCRIPT"

echo " copy backup to host server to: $VM_BACKUP_DIR"
docker cp $DB_CONTAINER:$BACKUP $VM_BACKUP_DIR

# load image
echo "load docker image $DOCKER_IMAGE_PATH"
docker load < $DOCKER_IMAGE_PATH

echo "update API_IMAGE in .env"
sed -i~ "/^API_IMAGE=/s/=.*/=\"januszpol-bets-api:$TAG\"/" /home/admin/januszpol-bets-api/.env

# #run docker compose
echo "start services (docker compose up)"
docker compose -f /home/admin/januszpol-bets-api/docker-compose.prod.yml -p januszpol up -d

