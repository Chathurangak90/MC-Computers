# MC Computers - Invoice Management System

## **Technologies Used:**

- **Frontend:** Angular (19)
- **Backend:** .NET Web API
- **Database:** MS SQL Server
- **Entity Framework Core (EF Core)** for data interaction
- **Task-based Asynchronous Pattern (TAP)** for handling asynchronous methods

---

## **Key Features:**

### **1. Asynchronous Operations**
Improves responsiveness and performance in high-traffic environments by handling long-running operations asynchronously.

### **2. Data Integrity and Validation**
Implements proper error handling and validation mechanisms to ensure consistent and reliable data entry.

---

### **Installation Steps:**

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/Chathurangak90/MC-Computers.git
### **Backend Setup:**

- Navigate to the backend project directory.

- Update the connection string in appsettings.json with your MS SQL Server credentials.

### **Run database migrations to set up the database schema:**

You can update the database using the following command:

- Update-Database
- note that before the Update-Database in the NugetpackeManger-packgemanageconsole need to select default prject MCComputers.Repositories after select run Update-Database"

The API endpoints are now accessible via Swagger UI at: http://localhost:44313/swagger.

#### **Customer and Product Table Data Insertion:**
- To properly use the invoice generation system, both the Customer and Product tables need to be populated manually. Here’s how to do that:

#### **Invoice Data Creation:**
- Once the Customer and Product data is inserted, you can begin generating invoices, associating the selected products and customer with each transaction.

#### **Insert Customer Data:**
- Open the database and navigate to the Customer table.
- Insert data for customers. Here’s an example of how to insert customer and product details data into the database manually:
#### **Sql query for insert customer data**
INSERT INTO Customers (Name, Email, PhoneNumber, CreatedAt)
VALUES ('Ricky', 'ricky@gmail.com', '0111', GETDATE());

#### **Sql query for insert Product details data**

INSERT INTO Products (Name, Price, StockQuantity, CreatedAt)
VALUES 
('HardDisk', 5500.00, 50, GETDATE()),
('Ram', 1500.00, 100, GETDATE());

Ensure that the product information such as name, price, and stock quantity is inserted before generating an invoice. This will allow the system to reference products when creating invoices.

### **Frontend Setup:**

#### **Navigate to the frontend directory:**

- cd frontend
#### **Install dependencies:**

- npm install
#### **Start the Angular app:**

- ng serve
