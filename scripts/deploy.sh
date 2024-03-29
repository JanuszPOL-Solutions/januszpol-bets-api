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

sleep 10s

#print info about docker containers
echo "containers status"
docker ps --filter "name=januszpol"

echo "check statuses"
frontend_container_name="januszpol-frontend-1"
db_container_name="januszpol-db-1"
api_container_name="januszpol-api-1"
nginx_container_name="nginx-proxy"
nginx_acme_container_name="januszpol-nginx-proxy-acme-1"

if [ $(docker inspect -f '{{.State.Running}}' $db_container_name) = "true" ] && [ $(docker inspect -f '{{.State.Running}}' $api_container_name) = "true" ] && [ $(docker inspect -f '{{.State.Running}}' $nginx_container_name) = "true" ] && [ $(docker inspect -f '{{.State.Running}}' $nginx_acme_container_name) = "true" ] && [ $(docker inspect -f '{{.State.Running}}' $frontend_container_name) = "true" ]
then
    echo "all containers are running"
else 
    echo "Some containers failed to start, check logs on the server" >&2
    exit 1
fi
