# Clinic Management System

At the end of my Master’s second year, I developed a clinic management system utilizing microservices architecture, deployed using Docker. The system comprises seven microservices, each operating independently to provide various services. They are implemented using different programming languages and technologies. These microservices communicate with one another through RESTful HTTP requests, and I implemented Service Discovery using the Consul library. This project was part of my thesis, demonstrating the significance of microservices architecture in enhancing a system's flexibility principles.

## Features
- Microservices architecture
- Deployed using Docker
- Service Discovery using Consul
- RESTful HTTP communication between services
- Developed with multiple programming languages and technologies

## Microservices
1. **Patient Service**: 
   - Includes CRUD operations (add, edit, delete) for the Patient model.
   - Implemented using Python Flask.
2. **Clinic Service**: 
   - Retrieves patient medical history (a list of medical records).
   - Performs CRUD operations for Medical Record, Doctor, and Prescription models.
   - Implemented using ASP.NET Web API.
3. **UI Service**: 
   - Provides a user interface for the system.
   - Implemented using Blazor Server.
4. **Bill Service**: 
   - Includes CRUD operations for the Bill model.
   - Implemented using ASP.NET Web API.
5. **Identity Service**: 
   - Manages user authentication and authorization.
   - Implemented using ASP.NET Web API.
6. **Appointment Service**: 
   - Includes CRUD operations for the Appointment model.
   - Implemented using ASP.NET Web API.
7. **Consul Service Discovery**: 
   - Acts as a service registry for the rest of the system.
   - Registers the IP and port of all services (except the UI service).
   - Used to retrieve the IP and port of the needed service to make requests.


## Architecture
![Architecture](https://github.com/AdamBazzal3/HealthcareMicroservices/blob/main/Architecture.png)

## Prerequisites

Before you can run the clinic management system, ensure you have the following installed:

1. **Docker**: The system is containerized using Docker. Install Docker from the [official Docker website](https://www.docker.com/get-started).

2. **Docker Compose**: Used to manage multi-container Docker applications. Docker Compose is typically included with Docker Desktop, but you can verify its installation and version by running:
    ```bash
    docker-compose --version
    ```

3. **Git**: To clone the repository. You can install Git from the [official Git website](https://git-scm.com/).

Once you have these prerequisites installed, you can proceed with the installation and usage instructions provided in the next sections.

## Installation
1. Clone the repository
    ```bash
    git clone https://github.com/AdamBazzal3/HealthcareMicroservices.git
    ```
2. Navigate to the project directory
    ```bash
    cd HealthcareMicroservices
    ```
3. Build the Docker containers
    ```bash
    docker-compose up --build
    ```

## Usage
1. Access the system through `http://localhost:8001`.

## Database Management
- The identity service uses MS SQL Server for database management.
- The rest of the services (except UI Service) use NoSql MongoDb database.

