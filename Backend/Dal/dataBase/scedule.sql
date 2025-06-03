-- מחיקת כל תורים ישנים (זהירות!)
TRUNCATE TABLE EmptyAppointments;


-- שלב 1: יצירת טווח של תאריכים 5 חודשים קדימה
WITH Dates AS (
    SELECT CAST(GETDATE() AS DATE) AS d
    UNION ALL
    SELECT DATEADD(DAY, 1, d)
    FROM Dates
    WHERE d < DATEADD(MONTH, 5, GETDATE())
),
-- שלב 2: צירוף ל-TherapistWorkingHours לפי יום בשבוע
TherapistSchedule AS (
    SELECT 
        d.d AS AppointmentDate,
        t.TherapistId,
        t.DayOfWeek,
        t.StartTime,
        t.EndTime
    FROM Dates d
    JOIN TherapistWorkingHours t
        ON DATEPART(WEEKDAY, d.d) = t.DayOfWeek
),
-- שלב 3: יצירת תורים בני 20 דקות לכל תאריך
TimeSlots AS (
    SELECT 
        ts.TherapistId,
        ts.AppointmentDate,
        DATEADD(MINUTE, 20 * n.number, CAST(ts.AppointmentDate AS DATETIME) + CAST(ts.StartTime AS DATETIME)) AS AppointmentDateTime
    FROM TherapistSchedule ts
    JOIN master.dbo.spt_values n
        ON n.type = 'P' 
        AND n.number < DATEDIFF(MINUTE, ts.StartTime, ts.EndTime) / 20
),
-- שלב 4: בחירה של 30 תורים בלבד לכל מטפל (מפוזרים)
RankedAppointments AS (
    SELECT *,
        ROW_NUMBER() OVER (PARTITION BY TherapistId ORDER BY NEWID()) AS rn
    FROM TimeSlots
)
-- שלב 5: הכנסת 30 תורים שונים לכל מטפל
INSERT INTO EmptyAppointments (AppointmentDate, AppointmentTime, TherapistId)
SELECT 
    CAST(AppointmentDateTime AS DATE),
    CAST(AppointmentDateTime AS TIME),
    TherapistId
FROM RankedAppointments
WHERE rn <= 30
OPTION (MAXRECURSION 0);
