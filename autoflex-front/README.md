Autoflex Manager - Production Intelligence System
This project was developed as a technical assessment for Projedata. It consists of a full-stack web application designed to manage raw materials, products, and calculate production capacity based on available stock.

 Features
Full CRUD for Products & Raw Materials: Register, list, update, and delete components and final goods.

Material Association: Link specific raw materials to products with defined required quantities.

Production Intelligence: An algorithm that analyzes current stock levels and recipes to suggest how many units of each product can be manufactured.

 Tech Stack
Backend: .NET 8 Web API following the Repository Pattern and Domain-Driven Design (DDD) principles.

Frontend: React with modern Hooks and Axios for seamless API integration.

Database: Relational Database managed via Entity Framework Core.

Architecture: Decoupled Client-Server architecture.

Project Structure
The repository is organized as a monorepo:

/Autoflex.Api: The entry point for the backend service.

/Autoflex.Domain: Core entities and business logic.

/Autoflex.Infrastructure: Data persistence and database context.

/autoflex-front: The React frontend application.

⚙️ How to Run
Backend: Open AutoflexManager.slnx in Visual Studio and run the project.

Frontend: Navigate to /autoflex-front, run npm install and then npm run dev.

API Access: The frontend is configured to communicate with the API at https://localhost:7154/api.
