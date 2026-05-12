# RuleWay E-Commerce API

This project was developed as a .NET 8 Web API for the RuleWay e-commerce merchandising management case.

The main goal is to manage products and categories and apply the product live status rules described in the case document.

## Technologies

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger

## Features

- Product CRUD
- Category CRUD
- Product filtering
- Category based minimum stock control
- Automatic IsLive calculation

## Business Rules

- Product title is required and can be maximum 200 characters.
- A product belongs to one category.
- A product must have a category to be live.
- Each category has a minimum stock quantity.
- If product stock is lower than the category minimum stock, IsLive becomes false.
- If product stock is equal to or greater than the category minimum stock, IsLive becomes true.
- IsLive is calculated in the backend and is not sent by the client.
- If a category minimum stock value is updated, related products are recalculated.
- A category cannot be deleted if it has products.

## Endpoints

### Categories

| Method | Endpoint |
|---|---|
| GET | /api/categories |
| GET | /api/categories/{id} |
| POST | /api/categories |
| PUT | /api/categories/{id} |
| DELETE | /api/categories/{id} |

### Products

| Method | Endpoint |
|---|---|
| GET | /api/products |
| GET | /api/products/{id} |
| POST | /api/products |
| PUT | /api/products/{id} |
| DELETE | /api/products/{id} |
| GET | /api/products/filter |

## Filter Example

```
GET /api/products/filter?keyword=phone&minStock=5&maxStock=100
```

The keyword is searched in product title, product description and category name.

## Example Requests

Create category:

```json
{
  "name": "Electronics",
  "minimumStockQuantity": 10
}
```

Create product (stock below minimum, IsLive will be false):

```json
{
  "title": "iPhone 15",
  "description": "Apple smartphone",
  "categoryId": 1,
  "stockQuantity": 5
}
```

Create product (stock above minimum, IsLive will be true):

```json
{
  "title": "Samsung S24",
  "description": "Android smartphone",
  "categoryId": 1,
  "stockQuantity": 15
}
```

## How to Run

Restore packages:

```
dotnet restore
```

Apply migration:

```
Update-Database
```

Or with .NET CLI:

```
dotnet ef database update
```

Run the project:

```
dotnet run
```

Open Swagger:

```
https://localhost:{port}/swagger
```

## Notes

- SQLite is used to make the project easy to run locally.
- The database file is created after applying migrations.
- IsLive is never accepted from the client. It is always calculated by the backend.
