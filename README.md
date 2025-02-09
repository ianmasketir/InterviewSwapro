# InterviewSwapro

Instruction:

0. Restore the database "TesSwapro.bak" or run the script "DbScript.txt". Change the connection string according to your server db connection.
1. Run/deploy PORECT.API first, then run/deploy PORECT.
2. Please change the "BaseUrl" section in the apps file "appsettings.json" with following:
2a. If API deployed on http, change the https://localhost:7278/ to the api location. Example: http://swaprodev:8322/ 1b. If API deployed on https, please deploy it in folder "tes" under "Default Web Site". Change the https://localhost:7278/ to the api location. Example: https://swaprodev/tes/
3. User must login to use this application.
4. If user not yet have account, please register it first. Either as "Admin" or "Customer".
5. User with role "Admin" can:
- Create Room
- Upload Room
- Upload Room Booking
- Enable/Disable Room
- Edit Room Detail
- Download Room Booking Report based on a period
- Checkout a room on behalf of customer that reject their booking
6. If user want to Upload Room for bulk create room data, user must download the provided template with button "Download Template Room" or download file "Contoh File Upload Room.xlsx" from this git for data example.
7. If user want to Upload Room Booking for bulk create room booking data, user must download the provided template with button "Download Template Booking" or download file "Contoh File Upload Booking.xlsx" from this git for data example.
8. User with role "Customer" can:
- Book a room
- Remove room booking 
- Upload Room Booking
