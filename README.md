# 💼 Kenyan Worker Payroll System

An enterprise-ready, full-stack web application built with **Blazor Interactive Server** to automate employee payroll, calculate complex Kenyan statutory deductions, and generate official, print-ready financial documents.

## 🏗️ System Architecture

This project is structured using principles of **Clean Architecture** to ensure a strict separation of concerns, making the codebase scalable, testable, and maintainable.

  * **Domain Layer:** Contains the core business entities (`Employee`, `Payslip`, `StatutoryConfig`).
  * **Application Layer:** Houses the business logic and isolated services (`EmployeeService`, `PayrollCalculatorService`, `SettingsService`). The tax calculation engine operates independently of the UI.
  * **Infrastructure Layer:** Manages data persistence using **Entity Framework Core (EF Core)** and SQL database migrations.
  * **Presentation Layer (Components):** Built entirely with Blazor Server Razor components, relying on real-time SignalR communication for a seamless, single-page-application (SPA) feel.

## ✨ Core Features & Modules

### 1\. 🇰🇪 Kenyan Statutory Tax Engine

A fully automated, legally compliant tax calculation engine that converts Gross Pay to Net Pay in real-time. It accurately processes:

  * **PAYE (Income Tax):** Calculates tax brackets and automatically applies the standard KES 2,400 Personal Relief.
  * **NSSF Deductions:** Processes Tier 1 & Tier 2 limits.
  * **SHA (Social Health Authority):** Applies the 2.75% mandatory deduction.
  * **Affordable Housing Levy:** Applies the 1.5% mandatory deduction.

### 2\. ⚙️ Dynamic Statutory Configuration

Tax laws change, so the system is built to adapt without requiring code deployments. The **Settings Dashboard** allows HR Admins to directly update active KRA rates, NSSF limits, and relief amounts in the database, instantly updating all future payroll math across the application.

### 3\. 🖨️ Official Print-Ready Reporting

The system features a dedicated `PrintablePayslip` engine. By utilizing advanced CSS `@media print` rules, the web application intelligently strips away all navigation bars, sidebars, and interactive buttons when a user initiates a print command. This transforms a web view into a clean, corporate A4 document ready for physical printing or PDF generation.

### 4\. 👥 Employee & Overtime Management

  * **Expandable Data Tables:** The primary employee list utilizes a custom "accordion" UI. Clicking an employee smoothly slides open a hidden drawer containing their exact tax breakdown, eliminating the need to load separate pages.
  * **Automated Overtime:** The system automatically flags workers exceeding standard hours and dynamically calculates their bonus pay at a 1.5x premium rate.
  * **Live Search:** Fully asynchronous, real-time search filtering that maintains active tax calculations without dropping data.

### 5\. 🖩 Standalone Tax Calculator

A dedicated sandbox environment within the app where administrators can input hypothetical basic salaries to instantly preview the exact tax breakdown and Net Pay without altering actual employee database records.

### 6\. 📊 Interactive Dashboard

A dynamic home screen providing a high-level overview of company payroll health. It features animated statistical counters that calculate Total Headcount, Active Overtime Workers, and Total Company Payout in real-time on page load.

## 🎨 UI / UX Design Philosophy

The frontend is built entirely from scratch without relying on heavy external CSS frameworks like Bootstrap for the core layouts.

  * **Zero Inline Styling:** All components utilize a strictly maintained, global `app.css` stylesheet for absolute separation of structure and design.
  * **Responsive Layout:** Features a collapsible sidebar navigation and flexbox/grid-driven layouts that adapt to both desktop and mobile screens.
  * **Modern Aesthetics:** Utilizes soft gradient backgrounds, subtle box-shadows, glass-morphism effects on profile cards, and clean transition animations for interactive elements.

## 👨‍💻 Developer

Designed and developed by **Ian Obino**.

  * **Email:** nyamosiobino@gmail.com
  * **Portfolio:** [nyam0s1.github.io](https://nyam0s1.github.io/)
