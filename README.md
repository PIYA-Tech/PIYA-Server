# PIYA Backend API

A Pharmacy Information & Location API built with ASP.NET Core 9.0 and PostgreSQL.

## Progress: ![Progress](https://geps.dev/progress/5)

---

## Features to Implement

### Authentication & Security

- [ ] **Fix JWT Token Generation**
- [ ] **Implement Password Hashing**
- [ ] **Add JWT Authentication Middleware**
- [ ] **Implement Token Validation**
- [ ] **Fix Token Expiration Access**
- [ ] **Add Refresh Token Logic**

### Configuration

- [ ] **Create appsettings.json Template**
- [ ] **Add Connection String Documentation**
- [ ] **Environment-Specific Settings**

---

### High Priority

### User Management

- [ ] **UserService.Authenticate()**
- [ ] **UserService.Create()**
- [ ] **UserService.GetById()**
- [ ] **UserService.Update()**
- [ ] **UserService.Delete()**
- [ ] **Create UserController**
- [ ] **Create AuthController**

### Search & Geolocation

- [ ] **SearchService.SearchByCountry()**
- [ ] **SearchService.SearchByCity()**
- [ ] **SearchService.SearchByRadius()**
- [ ] **CoordinatesService.GetCountry()**
- [ ] **CoordinatesService.GetCity()**
- [ ] **CoordinatesService.CalculateDistance()**
- [ ] **CoordinatesService CRUD Operations**

### Pharmacy Company Management

- [ ] **PharmacyCompanyService.GetById()**
- [ ] **PharmacyCompanyService.Create()**
- [ ] **PharmacyCompanyService.Update()**
- [ ] **PharmacyCompanyService.Delete()**
- [ ] **PharmacyCompanyService.GetAll()**
- [ ] **PharmacyCompaniesController Endpoints**

---

### Medium Priority

### API Improvements

- [ ] **Add Model Validation**
- [ ] **Global Exception Handling**
- [ ] **Add Logging**
- [ ] **API Versioning**
- [ ] **Response DTOs**
- [ ] **Pagination**
- [ ] **Filtering & Sorting**

### Authorization & Roles

- [ ] **Role-Based Authorization**
- [ ] **User Role Assignment**
- [ ] **Policy-Based Authorization**
- [ ] **Pharmacy Manager Assignment**
- [ ] **Staff Management**

### Data Enhancements

- [ ] **Pharmacy Operating Hours**
- [ ] **Pharmacy Contact Info**
- [ ] **Pharmacy Services**
- [ ] **Pharmacy Ratings**
- [ ] **Search History**

---

### Low Priority

### Advanced Features

- [ ] **Email Verification**
- [ ] **Password Reset Flow**
- [ ] **Two-Factor Authentication**
- [ ] **File Upload**
- [ ] **Caching**
- [ ] **Rate Limiting**
- [ ] **CORS Configuration**
- [ ] **Health Check Endpoints**

### Integration & External Services

- [ ] **Google Maps API Integration**
- [ ] **Email Service**
- [ ] **SMS Service**
- [ ] **Export to PDF**
- [ ] **Webhook Support**

### Testing & Documentation

- [ ] **Unit Tests**
- [ ] **Integration Tests**
- [ ] **Swagger Annotations**
- [ ] **XML Documentation**
- [ ] **Postman Collection**
- [ ] **Architecture Documentation**

---

## Known Bugs to Fix

1. **ID Type Inconsistency**
2. **Async/Await Issues**
3. **Missing SaveChangesAsync**
4. **No Input Validation**
5. **Foreign Key Constraints**

---

## Architecture Components

### Models

- [x] Pharmacy
- [x] PharmacyCompany
- [x] User
- [x] Token
- [x] Coordinates

### Services (Interfaces + Implementations)

- [ ] PharmacyService (90% complete)
- [ ] PharmacyCompanyService (empty)
- [ ] UserService (stub only)
- [ ] JwtService (broken)
- [ ] SearchService (not implemented)
- [ ] CoordinatesService (not implemented)

### Controllers

- [ ] PharmacyController (partial)
- [ ] PharmacyCompaniesController (empty)
- [ ] UserController (missing)
- [ ] AuthController (missing)

### Database

- [x] PostgreSQL with EF Core
- [x] Initial migration created
- [x] DbContext configured

---

## Tech Stack

- **Framework:** ASP.NET Core 9.0
- **Database:** PostgreSQL 15+
- **ORM:** Entity Framework Core 9.0
- **Documentation:** Swagger/OpenAPI
- **Authentication:** JWT (in progress)