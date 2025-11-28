# Guía de Pruebas de Servicios (Endpoints)

Esta guía detalla cómo probar cada uno de los servicios del API `GestionBelleza`, incluyendo los payloads (JSON) requeridos y las respuestas esperadas.

**Nota:** Para los endpoints protegidos (candado en Swagger), debes incluir el Token JWT en el header `Authorization`.
Formato: `Bearer <TU_TOKEN>` (En Swagger solo pega el token sin la palabra Bearer).

---

## 1. Autenticación (Auth)

### 🔐 Login
*   **Endpoint:** `POST /Auth/login`
*   **Descripción:** Obtiene el token de acceso.
*   **Body (JSON):**
    ```json
    {
      "email": "admin@gestionbelleza.com",
      "password": "Admin123!"
    }
    ```
*   **Respuesta Esperada (200 OK):**
    ```json
    {
      "id": 1,
      "nombreCompleto": "Administrador Sistema",
      "email": "admin@gestionbelleza.com",
      "rol": "admin",
      "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9..."
    }
    ```

---

## 2. Gestión de Clientes

### 📄 Listar Clientes
*   **Endpoint:** `GET /Cliente`
*   **Descripción:** Obtiene todos los clientes registrados.
*   **Respuesta Esperada (200 OK):** Lista de clientes.

### ➕ Crear Cliente
*   **Endpoint:** `POST /Cliente`
*   **Descripción:** Registra un nuevo cliente.
*   **Body (JSON):**
    ```json
    {
      "nombre": "Valeria",
      "apellido": "Montoya",
      "telefono": "3001234567",
      "correoElectronico": "valeria@mail.com",
      "alias": "Vale",
      "cedula": "1020304050"
    }
    ```

---

## 3. Gestión de Servicios

### 📄 Listar Servicios
*   **Endpoint:** `GET /Servicio`
*   **Descripción:** Obtiene el catálogo de servicios.
*   **Respuesta Esperada (200 OK):** Lista de servicios con sus precios y duraciones.

### ➕ Crear Servicio (Solo Admin)
*   **Endpoint:** `POST /Servicio`
*   **Body (JSON):**
    ```json
    {
      "nombreServicio": "Keratina",
      "descripcion": "Alisado permanente sin formol",
      "precio": 120000,
      "duracionEstimadaMin": 120,
      "categoria": "Cabello"
    }
    ```

---

## 4. Gestión de Citas (Core)

### 📅 Crear Cita (Solo Admin)
*   **Endpoint:** `POST /Cita`
*   **Descripción:** Agenda una nueva cita validando disponibilidad y sumando tiempos.
*   **Body (JSON):**
    ```json
    {
      "idCliente": 1,
      "idUsuario": 2, 
      "fechaCita": "2025-12-01",
      "horaInicio": "14:00:00",
      "notasAdicionales": "Cliente prefiere productos sin olor",
      "serviciosIds": [1, 3] 
    }
    ```
    *(Nota: `idUsuario` 2 corresponde a una de las especialistas creadas por el Seed, ej: Sandra. `serviciosIds` [1, 3] son Manicure y Pedicure).*

*   **Respuesta Esperada (201 Created):**
    Devuelve el objeto Cita con `horaFin` calculada automáticamente y `totalPrecio` sumado.

### 📄 Listar Citas
*   **Endpoint:** `GET /Cita`
*   **Descripción:** Muestra todas las citas con los nombres de Cliente y Especialista resueltos.
*   **Respuesta Esperada (200 OK):**
    ```json
    [
      {
        "id": 1,
        "nombreCliente": "Laura",
        "apellidoCliente": "Gomez",
        "nombreUsuario": "Sandra Especialista",
        "fechaCita": "2025-12-01",
        "horaInicio": "14:00:00",
        "horaFin": "15:45:00",
        "estado": "Pendiente",
        "totalPrecio": 90000,
        ...
      }
    ]
    ```

### 🔄 Actualizar Estado
*   **Endpoint:** `PATCH /Cita/{id}/estado`
*   **Body (JSON):**
    ```json
    {
      "estado": "Confirmada"
    }
    ```

---

## 5. Gestión de Usuarios (Especialistas)

### ➕ Crear Especialista (Solo Admin)
*   **Endpoint:** `POST /Usuario`
*   **Body (JSON):**
    ```json
    {
      "nombreCompleto": "Daniela Estilista",
      "correoElectronico": "daniela@gestionbelleza.com",
      "password": "Password123!",
      "telefono": "3159876543",
      "rol": "empleado"
    }
    ```

---

## 6. Pagos

### 💰 Registrar Pago
*   **Endpoint:** `POST /Pago`
*   **Body (JSON):**
    ```json
    {
      "idCita": 1,
      "metodoPago": "Nequi",
      "monto": 90000,
      "referenciaPago": "M123456"
    }
    ```
