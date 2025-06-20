# Forum

A forum platform with **automatic moderation** of offensive comments using ML.NET 2.0.

## Project Overview

This project implements a web application that **automates and improves the process of moderating forum content** by integrating modern machine learning (ML.NET) and a clean, responsive interface. The main goal is to automatically detect and flag offensive comments, provide a review system for moderators, and offer a robust admin panel for user and role management.

---

## Features

- **User Registration and Authentication**  
  Users can register and log in to participate in forum discussions.

- **Post Creation and Commenting**  
  Users can create forum posts and comment on them.

- **Automatic Comment Moderation**  
  Every comment is analyzed by a trained ML.NET model for offensive content. Flagged comments are held for moderator review.

- **Moderator Panel**  
  Moderators can approve or reject flagged comments, edit or delete posts and comments.

- **Admin Panel**  
  Admins manage user roles (User, Moderator, Admin) and can activate/deactivate user accounts.

- **Responsive UI**  
  Built with ASP.NET Razor Views, Bootstrap, and custom CSS for desktop and mobile usability.

---

## Technologies Used

- **Backend:** ASP.NET Core MVC (.NET 9+)
- **Frontend:** Razor Views, Bootstrap 5, CSS
- **Database:** Microsoft SQL Server 2022 (runs in a Docker container)
- **Authentication:** ASP.NET Identity (roles: User, Moderator, Admin)
- **Machine Learning:** ML.NET 2.0, custom sentiment analysis model (`SdcaLogisticRegression`)
- **Other:** Entity Framework Core

---

## Database Design

**Main tables:**
- `AspNetUsers`: Stores registered users and their status.
- `AspNetRoles`, `AspNetUserRoles`: Role management (admin, moderator, user).
- `Posts`: Forum topics (title, content, author, timestamp).
- `Comments`: Comments on posts (content, author, flag for offensiveness, approval status).

**ER Diagram:**  
*(see `er-diagram.png`)*
![ER Diagram](diagram.png)

---

## AI & Machine Learning

- **Model:**  
  The app uses a custom-trained ML.NET model for sentiment analysis, based on a manually curated dataset (`comments.tsv`). The model uses the `SdcaLogisticRegression` algorithm ([details here](https://www.jmlr.org/papers/volume14/shalev-shwartz13a/shalev-shwartz13a.pdf)) for binary classification (offensive / not offensive).

- **Training:**  
  Run `dotnet run -- --train` to (re)train the model from the TSV data. The model is saved to `SentimentModel.zip`.

- **Usage:**  
  On every comment submission, the text is sent to the ML.NET model via the `SentimentService`. If the result is `True` (offensive), the comment is flagged and requires moderator approval.

---

## Project Structure

- `/Controllers`  
  - `AccountController`: Registration, login, user activity check.
  - `PostController`: CRUD operations for forum posts.
  - `CommentController`: Comment submission, AI flagging.
  - `ModeratorController`: Approve/reject flagged comments, edit/delete posts/comments.
  - `AdminController`: User management, account activation/deactivation, role management.

- `/Models`  
  - `ApplicationUser`: Extends IdentityUser with "active" status.
  - `Post`: Forum post structure.
  - `Comment`: Comment structure with moderation/flagging fields.

- `/Views`  
  - Razor Views for dynamic HTML rendering.

- `/Data`  
  - `ApplicationDbContext.cs`: Entity Framework context, DB connection and schema setup.

- `/Services`  
  - `SentimentService.cs`: Loads and uses the ML.NET model for comment classification.
  - `ModelTraining.cs`: Prepares data and trains the ML.NET model.

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- Microsoft SQL Server image

### Setup

1. **Clone the repository:**  
   ```bash
   git clone https://github.com/ant0nlo/ForumSentiment.git
   cd ForumSentiment

2. **Start the SQL Server in Docker:**  
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd"
   -p 1433:1433 --name forumsentimentdb -d mcr.microsoft.com/mssql/server:2022-latest

3. **Apply database migrations:**  
   ```bash
   dotnet ef database update

4. **Run the application:**  
   ```bash
   dotnet run
