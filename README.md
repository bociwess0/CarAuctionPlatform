🚗 Car Auction Platform

A fully dockerized web application for car auctions, allowing users to register, bid, and manage listings in a seamless environment.


🧱 Tech Stack

Frontend: React / TypeScript

Backend: .NET Core Web API

Authentication: IdentityServer / OAuth

Containerization: Docker + Docker Compose



1. 📥 Clone the Repository
```
git clone https://github.com/bociwess0/CarAuctionPlatform.git
cd CarAuctionPlatform
```

2. 🛠 Update Your Hosts File
Before running the app, add the following line to your /etc/hosts (Linux/Mac) or C:\Windows\System32\drivers\etc\hosts (Windows):

```
127.0.0.1 id.car-auction-platform.local app.car-auction-platform.local api.car-auction-platform.local
```


🔐 This step allows your browser to resolve the local domain names correctly.


3. 🐳 Start the Docker Containers
In the root of the project, run:


```
docker-compose up --build
```


This will:

- Build and start the frontend, backend, and identity services

- Run everything in isolated containers

- Expose the platform under local domains


4. 🚀 Access the Application

Once Docker containers are running, you can access the platform via:

🌐 Main App: http://app.car-auction-platform.local

Other services (API and Identity) are also accessible under their respective local subdomains.

📌 Notes

- Make sure ports used in docker-compose.yml are free.

- This setup is for local development only.

- For production or staging, domain/DNS settings must be configured externally.

