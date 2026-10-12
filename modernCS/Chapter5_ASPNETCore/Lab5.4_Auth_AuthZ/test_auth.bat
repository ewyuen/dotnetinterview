@echo off
echo Testing Authentication API...
echo.

echo 1. Testing user registration...
curl -X POST "http://localhost:5000/api/auth/register" ^
     -H "Content-Type: application/json" ^
     -d "{\"email\":\"test@example.com\",\"password\":\"Test123!\",\"firstName\":\"John\",\"lastName\":\"Doe\"}"
echo.
echo.

echo 2. Testing user login...
curl -X POST "http://localhost:5000/api/auth/login" ^
     -H "Content-Type: application/json" ^
     -d "{\"email\":\"test@example.com\",\"password\":\"Test123!\"}"
echo.
echo.

echo 3. Testing admin login...
curl -X POST "http://localhost:5000/api/auth/login" ^
     -H "Content-Type: application/json" ^
     -d "{\"email\":\"admin@authauthz.com\",\"password\":\"Admin123!\"}"
echo.
echo.

echo Testing completed.
pause