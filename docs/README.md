
# System Components and Classes Description

## Components and Classes Overview

### Component: Product
- Represents items for sale in the marketplace, handling their attributes like name, price, and discounts.

### Component: Store
- Manages details about retail outlets, including their operational status, contact information, and identification.

### Component: Client
- Tracks client-related data such as login status and identification, essential for user management and authentication.

### Component: Basket
- Stores and manages a collection of selected items, detailing quantities and product IDs for checkout processes.

### Class: MarketFacade
- Serves as a simplified interface for interacting with more complex market system operations, embodying the facade design pattern.

### Class: Alert
- An abstract class designed to manage notifications and alerts within the system, focusing on their delivery and read status.

### Interface: PermissionsService
- Defines a set of methods for managing and checking permissions within the system, crucial for access control and service management.

### Class: SupplyManager
- Oversees inventory and stock levels, ensuring products are available for sale and managing relationships with suppliers.

### Component: Delivery
- Handles the logistics of delivering orders to customers, including tracking, route optimization, and delivery status updates.

### Class: PaymentManager
- Manages financial transactions, overseeing payment processing, validation, and security of payment data.

### Class: Transaction
- Facilitates and records the exchange of goods and services for payment, ensuring accuracy and integrity in financial operations.

### Class: PermissionManager
- Controls access rights within the system, determining who can perform specific actions based on roles and permissions.

### Strategy: PurchaseStrategy
- Defines various purchasing tactics and strategies, allowing dynamic changes to how purchases are handled based on business rules or customer behavior.

### Component: Order
- Manages order life cycles from creation through completion, including details such as order contents, status, and customer information.
