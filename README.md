# 🧒 Emotional Therapy Site

**אתר ניהול תורים למכון טיפולים רגשיים לילדים** – מאפשר למטפלים ולקוחות לנהל את הזמינות, לקבוע ולבטל תורים לפי הרשאות.

---

## 🛠️ טכנולוגיות עיקריות:
- **Frontend**: React + TypeScript
- **Backend**: ASP.NET Core Web API (C#)
- **Database**: SQL Server
- **Cloud**: Google Cloud Run + Cloud Storage (בקרוב)
- **DevOps**: Docker, Docker Compose

---

## ✨ פיצ'רים עיקריים:
- כניסה / הרשמה עם אימות סיסמה
- צפייה בתורים פנויים ותפוסים
- קביעת תור חדש לפי סוג טיפול וזמינות
- ביטול תורים
- הפרדת הרשאות: לקוח / מטפל / אדמין
- סינון מטפלים לפי סוג טיפול
- מבנה של 3 שכבות: DAL, BL, API
- תיעוד קוד מסודר לפי תקני SOLID

---

## 🚀 הוראות הרצה:

### שרת:
```bash
cd Server
dotnet restore
dotnet run
