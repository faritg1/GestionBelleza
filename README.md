# Sistema de Gestión de Belleza - Backend (.NET)

## 📋 Descripción del Proyecto
Sistema integral para la gestión administrativa y operativa de un centro de estética/belleza. [cite_start]El sistema centraliza el agendamiento de citas, la gestión de clientes, el catálogo de servicios y la facturación, cumpliendo con los requerimientos definidos en la Matriz de Requerimientos Integrados[cite: 1].

## 🛠 Tech Stack
* **Lenguaje:** C# (.NET Core 8+)
* **Framework Web:** ASP.NET Core Web API
* **Base de Datos:** MySQL / MariaDB (XAMPP)
* **ORM:** Entity Framework Core
* **Seguridad:** JWT (Json Web Tokens) & Hashing (BCrypt)
* [cite_start]**Arquitectura:** Mobile First (Optimizado para iPhone 16 Pro o superior) [cite: 2]

---

## 🚀 Módulos y Funcionalidades (Requerimientos)

### 1. Gestión de Seguridad y Usuarios (Auth)
* [cite_start]**Rol Administrativo:** El sistema cuenta con un único rol de **Administrador** (RF B2)[cite: 2].
* **Login Seguro:** Acceso mediante correo y contraseña. [cite_start]Las contraseñas se almacenan encriptadas (hash) (RNF5)[cite: 2].
* [cite_start]**Gestión de Contraseñas:** Funcionalidad para cambio y recuperación de contraseña (RF B5)[cite: 2].

### 2. Gestión de Clientes
* [cite_start]**Registro Completo:** Datos obligatorios: Nombre, Apellido, Teléfono, Correo, **Alias** y **Cédula** (RF B1)[cite: 2].
* [cite_start]**CRUD:** Creación, lectura y actualización de perfiles de clientes (RF B3)[cite: 2].
* [cite_start]**Historial:** Visualización del historial de citas y ventas por cliente (RF D2, D5)[cite: 2].

### 3. Gestión de Servicios (Catálogo)
* [cite_start]**Datos del Servicio:** Nombre, descripción, precio y **duración estimada** (RF C1, C2)[cite: 2].
* [cite_start]**Precios Dinámicos:** El administrador puede actualizar los precios en cualquier momento (RF C3)[cite: 2].
* **Duración:** Campo crítico para calcular la disponibilidad de la agenda.

### 4. Gestión de Citas (Core)
* [cite_start]**Agendamiento Exclusivo:** Solo el administrador puede programar nuevas citas (RF A1)[cite: 2].
* [cite_start]**Selección Múltiple:** Una cita puede incluir uno o varios servicios (ej. Manicure + Pedicure) (RF A3)[cite: 2].
* [cite_start]**Cálculo de Tiempos:** El sistema suma la duración de todos los servicios seleccionados para bloquear el tiempo en la agenda (RF A8)[cite: 2].
* [cite_start]**Validación de Disponibilidad:** El sistema valida que no existan cruces de horarios antes de confirmar (RF A5, A6)[cite: 2].
* [cite_start]**Estados de Cita:** Pendiente, Confirmada, En curso, Finalizada (RF D1)[cite: 2].
* [cite_start]**Asignación:** Se registra el ID de la especialista que realiza el servicio (RF A4)[cite: 2].

### 5. Facturación y Reportes
* [cite_start]**Pagos:** Registro de pagos (Efectivo, NEQUI, Transferencia) mostrando el valor total (RF D3)[cite: 2].
* [cite_start]**Reportes:** Generación de reportes diarios, semanales y mensuales (ventas y servicios más vendidos) (RF D4)[cite: 2].

---

## ⚙️ Reglas de Negocio (Lógica de Servicios .NET)

> Estas reglas deben implementarse en la capa de **Services** para garantizar la integridad de los datos.

1.  **Restricción Horaria (RF A6):**
    Una especialista no puede tener dos citas asignadas en el mismo rango de hora.
    [cite_start]*Lógica:* `NuevaCita.Inicio < CitaExistente.Fin && NuevaCita.Fin > CitaExistente.Inicio` -> **Lanzar Excepción**. [cite: 2]

2.  **Notificaciones (RF A10):**
    [cite_start]El sistema debe estar preparado para disparar notificaciones (SMS/WhatsApp/Correo) con antelación configurable (ej. 24h). [cite: 2]

3.  **Registro de Responsable (RF A4):**
    [cite_start]Al crear una cita, se debe capturar automáticamente el usuario logueado (claims del token) como responsable del servicio. [cite: 2]

---

## 📱 Requerimientos No Funcionales (RNF)

* [cite_start]**Usabilidad:** Interfaz intuitiva que no requiera capacitación extensa (RNF1)[cite: 2].
* [cite_start]**Diseño Visual:** Estilo basado en colores **PASTEL** (RNF2)[cite: 2].
* [cite_start]**Mobile First:** Diseño responsivo y optimizado para móviles (RNF 3, RNF 4)[cite: 2].
* [cite_start]**Disponibilidad:** Arquitectura robusta para asegurar disponibilidad 99% (RNF5)[cite: 2].
* [cite_start]**Mantenibilidad:** Código modularizado y documentado (RNF6)[cite: 2].

---

## 🗄️ Modelo de Datos (Resumen)

El sistema utiliza una base de datos relacional (MySQL) con las siguientes entidades principales:

* `Usuarios` (Admin/Especialistas)
* `Clientes` (Alias, Cédula, Contacto)
* `Servicios` (Precio, Duración, Categoría)
* `Citas` (Fecha, Hora, Estado, Totales)
* `CitaServicios` (Tabla intermedia: 1 Cita tiene N Servicios)
* `Pagos` (Método de pago, Monto)

---
