# Dictionary

A foreign language console-based dictionary written in **C# (.NET 8)**. Active using of Collections, LINQ  
Supports basic funtionality (add/delete/view words), supports training, progress stats, export progress to json/csv


---

## 🚀 Features
- ➕ Add word
- ➖ Remove word
- View words
- training (quiz)
- statistics 
- export/import dictionary
- save stats to file
- word categories division
- 📜 Clean and readable C# code  

---

## 📂 Project Structure
Dictionary/
│── Program.cs # Entry point
│── Word.cs # a word entity
│── DictionaryManager.cs # a Dictionary manager
|── Exceptions/*.cs # Errors catching
|── Trainer.cs # Trainer manager
│── README.md

## ⚙️ Installation & Run
### 1. Clone the repository:
```bash
git clone https://github.com/m01ves2/ConsoleCalculator.git
cd Dictionary

## 📂 Build and run
dotnet run --project Dictionary

## 📌 Usage Example
The program asks the user for input and performs the operation:


## 🗺️ Roadmap
- MVP/MVC architecture
- GUI version with Blazor or WinForms