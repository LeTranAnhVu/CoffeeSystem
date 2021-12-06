
#### Start mssql server
```shell
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Pa55w0rdd" -e "MSSQL_PID=Express" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest
```
