# Monthly Premium Calculator

## Solution Overview
This application provides a monthly premium calculation service with Angular 20 frontend and .NET 8 backend.

## Features
- Occupation-based premium calculation
- Real-time premium updates on form changes
- Comprehensive form validation
- Error handling and logging
- Responsive UI design

## Assumptions & Clarifications

### Business Logic
1. **Age Calculation**: Used `AgeNextBirthday` from input for calculation
2. **Date Format**: MM/YYYY format for Date of Birth
3. **Premium Formula**: (Death Cover * Occupation Rating Factor * Age) / (1000 * 12)
4. **Occupation Ratings**: Predefined mapping with factors as provided

### Technical Decisions
1. **Backend**: .NET 8 Web API with dependency injection
2. **Frontend**: Angular 20 with reactive forms
3. **Logging**: Serilog for backend, console logging for frontend
4. **Error Handling**: Custom exceptions and global error handling
5. **CORS**: Configured for Angular development server

### Validation Rules
1. **Name**: Required, max 100 characters
2. **Age**: 18-100 years
3. **Date of Birth**: MM/YYYY format, cannot be future date
4. **Occupation**: Required, must be from predefined list
5. **Death Sum Insured**: $1,000 - $10,000,000

## Setup Instructions

### Backend (.NET 8)
1. Navigate to backend directory
2. Run `dotnet restore`
3. Run `dotnet run`
4. API available at `https://localhost:7184`

### Frontend (Angular 20)
1. Navigate to frontend directory
2. Run `npm install`
3. Run `ng serve`
4. Application available at `http://localhost:4200`

## API Endpoints
- `GET /api/premium/occupations` - Get all occupations
<<<<<<< HEAD
- `POST /api/premium/calculate` - Calculate monthly premium
=======
- `POST /api/premium/calculate` - Calculate monthly premium
>>>>>>> 9be9b7a08631c5bd186374b659dd92617bfe51b8
