# FunBooksAndVideos

ASP.NET Core 8 Web API for the FunBooksAndVideos back-end task: an object-oriented model of the shop, a flexible
purchase order processor, the two business rules (BR1 and BR2), IoC throughout and Swagger UI.

## Run it

1. Open `FunBooksAndVideos.sln` in Visual Studio 2022 (17.8+ with the .NET 8 SDK).
2. Set **FunBooksAndVideos.Api** as the startup project and press F5. Swagger opens at `/swagger`.
3. Run the tests from **Test Explorer**, or from the command line:

```
dotnet test
```

## Solution layout (Clean Architecture)

| Project | Responsibility | Depends on |
|---|---|---|
| `Domain` | Entities and invariants: `Product` (`Book`, `Video`, `MembershipProduct`), `Customer`, `PurchaseOrder`, `PurchaseOrderItem`, `ShippingSlip` | nothing |
| `Application` | Use cases, the purchase order processor, business rules, repository interfaces, DTOs | Domain |
| `Infrastructure` | In-memory repositories and seed data | Application |
| `Api` | REST controllers, Swagger, ProblemDetails error handling, composition root | Application, Infrastructure |
| `Tests` | xUnit tests: domain, rules, processor, IoC wiring and HTTP integration tests | all |

## Purchase order processor

`PurchaseOrderProcessor` receives every registered `IPurchaseOrderRule` from the IoC container
(Strategy pattern + Chain-style pipeline). For each rule it calls `IsApplicable` and then `ApplyAsync`. Rules report what
they did as `RuleOutcome`s, and those outcomes come back in the API response.

* **BR1** `MembershipActivationRule`: activates any purchased membership on the customer account immediately.
  `MembershipType` is a `[Flags]` enum, so Book Club + Video Club = Premium.
* **BR2** `ShippingSlipRule`: if any item is physical (`Product.IsPhysical`), it builds a `ShippingSlip` through
  `IShippingSlipFactory` and stores it.

To add BR3, write one class that implements `IPurchaseOrderRule` and add one line to
`Application/DependencyInjection.cs`. The processor does not change.

## SOLID

* **S**: Each class has one job: entities enforce invariants, each rule implements one business rule, the factory builds slips, repositories persist, services orchestrate, controllers handle HTTP.
* **O**: New rules and new product types are added by writing new classes. The processor and the existing products stay as they are.
* **L**: Code works with `Product`, and `Book`, `Video` and `MembershipProduct` can be used anywhere a `Product` is expected.
* **I**: Small, focused interfaces: one repository and one service per aggregate, and `IPurchaseOrderRule` has only what a rule needs.
* **D**: The Application layer owns the abstractions, and Infrastructure implements them. All wiring happens in the composition root (`Program.cs` → `AddApplication()` / `AddInfrastructure()`). `TimeProvider` is injected, so tests don't depend on the system clock.

## Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/products`, `/api/products/{id}` | Catalogue |
| GET | `/api/customers`, `/api/customers/{id}` | Customers and their current membership |
| POST | `/api/purchase-orders` | Create **and process** a purchase order. Returns 201 with the order and the rules that were applied |
| GET | `/api/purchase-orders`, `/api/purchase-orders/{id}` | Purchase orders |
| GET | `/api/purchase-orders/{id}/shipping-slips` | Shipping slips for an order |
| GET | `/api/shipping-slips/{id}` | Single shipping slip |

Errors come back as RFC 7807 ProblemDetails: 400 for validation or business rule errors, 404 for not found, 409 for an order that was already processed.

### The example order from the brief

The seed data reproduces it: product 1 is the video, 2 is the book and 3 is the Book Club membership. Customer 4567890 is
seeded, and the first PO id is 3344656.

```json
POST /api/purchase-orders
{ "customerId": 4567890, "items": [ { "productId": 1 }, { "productId": 2 }, { "productId": 3 } ] }
```

This returns PO **3344656** with a total of **48.50**. BR1 activates the Book Club membership, and BR2 generates a shipping slip that lists only the book.
`FunBooksAndVideos.Api.http` has these requests ready to run from Visual Studio.

## Notes

* Storage is in memory, so data resets when the app restarts. To use a database, swap the registrations in `Infrastructure/DependencyInjection.cs` for EF Core implementations.
* Videos are streamed online, so they are not physical. Books are physical.
