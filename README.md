# DataForSEO-code-challenge
Goal: Create a flexible system that automatically accepts links to a list of files, processes each file in parallel, captures all errors, and provides aggregated statistics, while still being able to easily expand to new data and metrics.

## Testing
To test solution you need to do following steps:
1. Run `Update-Database` command in **Package Manager Console**
2. Open Windows PoweShell go to repository folder and run following commands to run docker container:
   1. `docker-compose up -d`
   2. `docker container start data-for-seo-challenge-rabbit-mq`
3. Run Dispatcher and Worker applications.
4. Go to postman, paste following cURL and click send:
```
   curl --location 'https://localhost:7223/api/task' \
--header 'Content-Type: application/json' \
--data '{
    "Link":"https://downloads.dataforseo.com/dfs_test/1caaa605-6f79-40ed-a6d1-4fe4754c6274/list.txt"
}'
```
## Endpoints
```
GET https://localhost:7223/api/task - return all super tasks
```
```
POST https://localhost:7223/api/task - create and run super tasks
```
```
GET https://localhost:7223/api/task/{id} - return super task by id, example: https://localhost:7223/api/task/3da31f8e-d127-4aa2-b93d-08ddd7995750
```
```
GET https://localhost:7223/api/subtask/{id} - return subtask by id, example: https://localhost:7223/api/task/a3c20d97-bd2e-4273-9606-08ddd799576c
```
