# 💸 Employee Payroll System

### Automated Wage Processing for Casual Laborers

**The Employee Payroll System** is a dynamic single-page application (SPA) built with **Blazor Interactive Server**. It eliminates manual wage calculations by automating salary computation based on hourly rates and tracked hours.

---

## 📸 Application Previews

### 1. Employee Management Grid

*A responsive data table to view, edit, and delete employee records in real-time.*

### 2. Dynamic Data Entry

*Input validation and automatic state updates using Blazor's two-way binding.*

---

## 🚀 Key Features

### ⚡ Automated Salary Calculation

* **Logic:** Utilizes C# Computed Properties (`=>`) to instantly calculate `Total Salary` based on `Hourly Pay` × `Hours Worked`.
* **Accuracy:** Eliminates human error in manual multiplication for large workforces.

### 🔄 Full CRUD Operations

* **Create:** Onboarding new workers via a dedicated modal-style form.
* **Read:** Real-time fetching of Employee lists using a centralized Service.
* **Update:** Edit existing records with ID-based routing (`/edit/{guid}`).
* **Delete:** Instant removal of records from the system state.

### 🏗️ Architecture: Blazor Server

* **Interactive Mode:** Uses `@rendermode InteractiveServer` for a seamless, JavaScript-free experience.
* **Dependency Injection:** Implements a Singleton `WorkerService` to manage application state across different components.
* **SPA Routing:** Client-side navigation without page reloads using the `NavigationManager`.

---

## 🛠️ Technical Implementation

The system demonstrates core **.NET Enterprise patterns** applied to a web context.

* **Data Model:**
```csharp
// Encapsulated logic ensures data consistency
public double totalSalary => hourlyPay * hoursWorked;

```


* **Service Layer:**
* `WorkerService.cs` acts as the single source of truth.
* Uses **LINQ** for querying (`FirstOrDefault`) and list manipulation.


* **UI Components:**
* **Razor Syntax:** Mixes HTML and C# for dynamic rendering.
* **Event Handling:** `@onclick` delegates for CRUD actions.


---

## 👨‍💻 Author

**Ian Nyamosi**

* **Role:** .NET Developer
* LinkedIn Profile(https://www.google.com/search?q=https://linkedin.com/in/ian-nyamosi)

* Portfolio(https://www.google.com/search?q=https://github.com/nyam0s1)

---

*Built with .NET 8 and Blazor Server.*
