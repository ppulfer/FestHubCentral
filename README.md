# FestHub Central

A real-time festival gastronomy management system built with Blazor Server and PostgreSQL.

## Overview

FestHub Central is a comprehensive web application designed to manage food and beverage operations at festivals. It provides real-time monitoring of vendors, inventory, orders, and financial transactions.

## Features

### 🎪 Dashboard
- Real-time sales statistics
- Vendor status map (15 location spots)
- Active alerts display
- Top-selling products
- Live updates via SignalR

### 🏪 Vendor Management
- Add/edit/delete vendors
- Assign to physical location spots (1-15)
- Toggle vendor status (open/closed)
- Contact information management
- Categorization (Food/Beverage/Mixed)

### 📦 Inventory Management
- Real-time stock level monitoring
- Color-coded stock indicators (Good/Warning/Critical)
- Low stock alerts
- Quick restock functionality
- Per-vendor inventory filtering

### 🧾 Order Processing
- Create new orders with multiple items
- Support for multiple payment methods (Cash/Card/Festival Tokens)
- Automatic inventory updates
- Order history with filtering
- Daily sales analytics

### 💰 Cash Register Management
- Open/close daily registers
- Automatic sales calculation
- Discrepancy detection
- Multi-payment method tracking
- End-of-day reconciliation

### 🚨 Alert System
- Automatic low-stock alerts
- Critical inventory notifications
- Alert resolution tracking
- Real-time push notifications

## Technology Stack

- **Frontend**: Blazor Server (.NET 10)
- **Backend**: ASP.NET Core
- **Database**: PostgreSQL with Entity Framework Core
- **Real-time**: SignalR
- **UI Framework**: Bootstrap 5

## Project Structure

```
FestHubCentral/
├── src/
│   └── FestHubCentral.Web/
│       ├── Components/
│       │   ├── Layout/          # Application layout
│       │   ├── Pages/           # Blazor pages
│       │   └── Shared/          # Reusable components
│       ├── Data/
│       │   ├── Models/          # Database models
│       │   └── ApplicationDbContext.cs
│       ├── Services/
│       │   ├── Interfaces/      # Service interfaces
│       │   └── *Service.cs      # Service implementations
│       ├── Hubs/
│       │   └── FestivalHub.cs   # SignalR hub
│       └── wwwroot/             # Static files
└── README.md
```

## Database Models

- **Vendor**: Festival vendors with location and contact info
- **Product**: Items sold by vendors
- **Inventory**: Stock tracking with min/max thresholds
- **Order/OrderItem**: Sales transactions
- **Alert**: System notifications
- **CashRegister**: Daily financial tracking

## Setup Instructions

### Prerequisites

- .NET 10 SDK (or later)
- PostgreSQL 12+
- Your favorite IDE (VS Code, Visual Studio, Rider)

### Database Setup

1. Ensure PostgreSQL is running
2. Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=festhubcentral;Username=postgres;Password=yourpassword"
  }
}
```

3. Apply migrations:

```bash
cd src/FestHubCentral.Web
dotnet ef database update
```

This will create the database and seed it with sample data (3 vendors, products, and inventory).

### Running the Application

```bash
cd src/FestHubCentral.Web
dotnet run
```

The application will be available at:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

## Default Data

The system comes pre-seeded with:
- 3 vendors (Burger Stand, Beer Garden, Pizza Corner)
- 4 products with pricing
- Initial inventory levels
- Sample data for testing

## Real-time Features

The application uses SignalR for real-time updates:
- New orders trigger dashboard updates
- Low stock generates immediate alerts
- Vendor status changes reflect instantly
- All connected clients receive updates simultaneously

## API Services

All business logic is encapsulated in services:

- `IVendorService`: Vendor operations
- `IProductService`: Product management
- `IInventoryService`: Stock tracking with auto-alerts
- `IOrderService`: Order processing with inventory updates
- `IAlertService`: Notification management
- `ICashRegisterService`: Financial reconciliation

## Color Coding

The UI uses color coding for quick status identification:
- 🟢 **Green**: Good stock, vendor open, no issues
- 🟡 **Yellow**: Low stock, warnings
- 🔴 **Red**: Critical stock, vendor closed, errors

## Future Enhancements

Potential additions for production use:
- User authentication and role-based access
- Mobile app for vendors
- Customer-facing ordering system
- Predictive inventory based on weather/attendance
- Payment terminal integration
- Multi-festival support
- Advanced reporting and analytics
- Email/SMS notifications

## Development

### Adding a New Feature

1. Create/update model in `Data/Models/`
2. Update `ApplicationDbContext.cs`
3. Create migration: `dotnet ef migrations add FeatureName`
4. Create service interface and implementation
5. Register service in `Program.cs`
6. Create Blazor component/page
7. Update navigation if needed

### Database Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove
```

## Troubleshooting

### Database Connection Issues
- Verify PostgreSQL is running
- Check connection string in `appsettings.json`
- Ensure database exists: `dotnet ef database update`

### SignalR Not Working
- Check browser console for connection errors
- Verify firewall allows WebSocket connections
- Ensure HTTPS is configured correctly

### Build Errors
- Clean the solution: `dotnet clean`
- Restore packages: `dotnet restore`
- Rebuild: `dotnet build`

## Contributing

This is a demonstration project showcasing modern .NET web development with Blazor Server, EF Core, and SignalR.

## License

This project is provided as-is for educational and demonstration purposes.

---

Built with ❤️ using Blazor Server, PostgreSQL, and SignalR
