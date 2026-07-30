/*Question 1*/

SELECT
    h."Name" AS "Hotel",
    COUNT(r."Id") AS "NumberOfReservations",
    SUM(r."TotalPrice") AS "Revenue"
FROM "Reservations" r
INNER JOIN "Hotels" h ON h."Id" = r."HotelId"
WHERE r."Status" = 'ACTIVE'
GROUP BY h."Name"
ORDER BY h."Name";

/*Question 2*/

SELECT
    c."FirstName",
    c."LastName"
FROM "Customers" c
LEFT JOIN "Reservations" r ON r."CustomerId" = c."Id"
WHERE r."Id" IS NULL
ORDER BY c."FirstName", c."LastName";

/*Question 3*/

SELECT
    h."Name" AS "Hotel",
    h."City" AS "City",
    SUM(r."TotalPrice") AS "Revenue"
FROM "Reservations" r
INNER JOIN "Hotels" h ON h."Id" = r."HotelId"
GROUP BY h."Id", h."Name", h."City"
ORDER BY "Revenue" DESC
LIMIT 5;