# Santander - Developer Coding Test

A RESTful API built with ASP.NET Core 8 to retrieve the details of the best `n` stories from the Hacker News API, ordered by score.
This project was designed with a focus on high performance and resilience.

## Performance & Resilience Strategy
To efficiently service a large number of requests without risking overloading the Hacker News API, the following strategies were implemented:

1. **Background Worker (`IHostedService`):** A background service runs periodically to fetch and update the stories. This ensures user requests never pay the latency cost of external HTTP calls.
2. **In-Memory Caching:** The processed, sorted array is stored in `IMemoryCache`. The controller reads directly from RAM (O(1) access time).
3. **Concurrency Control:** `Parallel.ForEachAsync` is used to fetch individual story details concurrently, with a strict `MaxDegreeOfParallelism` to avoid socket exhaustion.
4. **Polly Resilience:** The `HttpClient` is decorated with standard resilience handlers (Retry with Exponential Backoff and Circuit Breaker) to elegantly handle transient errors from the external API.

## Enhancements 
* **Authentication - Authorization
* **Integration Tests:** Implement end-to-end testing using `WebApplicationFactory` to validate the complete request lifecycle, ensuring that the controller, service, background worker, and caching mechanisms work seamlessly together.
* **Observability:** Introduce a comprehensive observability stack using OpenTelemetry, Serilog (for structured logging), and Prometheus/Grafana (for metrics and dashboards) to monitor API health, trace requests, and track the background worker's performance in real-time.
* **Distributed Caching (Redis):** Replace `IMemoryCache` with an `IDistributedCache` backed by Redis. In a scaled, multi-instance environment (like Kubernetes or cloud app services), this ensures all API replicas share a single centralized cache, preventing each node from executing redundant background fetches to the Hacker News API.
* **Extract background worker from this API project, to make this API just for "read" propouse.
* **Search by title using Lucene gen.
* **Imrove swagger documentation..


## How to Run the Application 

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Steps
1. Clone this repository.
2. Open a terminal and navigate to the root directory of the solution.
3. Run the following command to start the API:
   ```bash
   dotnet run --project Santander.HackerNews.Api/Santander.HackerNews.Api.csproj