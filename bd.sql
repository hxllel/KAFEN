USE master;

ALTER DATABASE KafenData SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

DROP DATABASE IF EXISTS KafenData;
GO

CREATE DATABASE KafenData;
GO

USE KafenData;
GO

---------------------------------------------------
-- Tabla TipoUsuario
---------------------------------------------------
CREATE TABLE TipoUsuario (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Descripcion NVARCHAR (MAX) NOT NULL
);
GO

---------------------------------------------------
-- Tabla Usuarios
---------------------------------------------------
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Nombre NVARCHAR (MAX) NOT NULL,
    Apellido_Paterno NVARCHAR (MAX),
    Apellido_Materno NVARCHAR (MAX),
    Correo_Electronico NVARCHAR (255) UNIQUE NOT NULL,
    Contraseña NVARCHAR (MAX) NOT NULL,
    FechaNacimiento DATETIME,
    tipo_UsuarioId INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE (),
    FOREIGN KEY (tipo_UsuarioId) REFERENCES TipoUsuario (Id)
);
GO

---------------------------------------------------
-- Categoría
---------------------------------------------------
CREATE TABLE Categoria (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Nombre NVARCHAR (MAX) NOT NULL
);
GO

---------------------------------------------------
-- Artículos
---------------------------------------------------
CREATE TABLE Articulos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    Nombre NVARCHAR (MAX) NOT NULL,
    Descripcion NVARCHAR (MAX),
    Precio DECIMAL(18, 2) NOT NULL,
    Stock INT DEFAULT 0,
    CategoriaId INT NOT NULL,
    VendedorId INT,
    FOREIGN KEY (CategoriaId) REFERENCES Categoria (Id),
    FOREIGN KEY (VendedorId) REFERENCES Usuarios (Id)
);
GO

---------------------------------------------------
-- Estatus
---------------------------------------------------
CREATE TABLE Estatus (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Descripcion NVARCHAR (MAX) NOT NULL
);
GO

---------------------------------------------------
-- Pedidos
---------------------------------------------------
CREATE TABLE Pedidos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    ClienteId INT NOT NULL,
    EstatusId INT NOT NULL,
    Fecha DATETIME DEFAULT GETDATE (),
    Total DECIMAL(18, 2),
    FOREIGN KEY (ClienteId) REFERENCES Usuarios (Id),
    FOREIGN KEY (EstatusId) REFERENCES Estatus (Id)
);
GO

---------------------------------------------------
-- DetalleVenta
---------------------------------------------------
CREATE TABLE DetalleVenta (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    IdPedidoId UNIQUEIDENTIFIER NOT NULL,        -- CORRECTO
    IdProductoId UNIQUEIDENTIFIER NOT NULL,    -- CORRECTO
    Cantidad INT NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (IdPedidoId) REFERENCES Pedidos (Id),
    FOREIGN KEY (IdProductoId) REFERENCES Articulos (Id)
);
GO

---------------------------------------------------
-- Carrito
---------------------------------------------------
CREATE TABLE Carrito (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID (),
    IdUsuarioId INT NOT NULL,                  -- CORRECTO
    IdArticuloId UNIQUEIDENTIFIER NOT NULL,
    Cantidad INT NOT NULL,
    FechaAgregado DATETIME DEFAULT GETDATE (),
    FOREIGN KEY (IdUsuarioId) REFERENCES Usuarios (Id),
    FOREIGN KEY (IdArticuloId) REFERENCES Articulos (Id)
);
GO

---------------------------------------------------
-- Inserts iniciales
---------------------------------------------------
INSERT INTO TipoUsuario (Descripcion) VALUES ('Cliente');
INSERT INTO TipoUsuario (Descripcion) VALUES ('Vendedor');
GO

INSERT INTO Estatus (Descripcion) VALUES ('Pendiente');
INSERT INTO Estatus (Descripcion) VALUES ('Confirmado');
INSERT INTO Estatus (Descripcion) VALUES ('Enviado');
INSERT INTO Estatus (Descripcion) VALUES ('Entregado');
INSERT INTO Estatus (Descripcion) VALUES ('Cancelado');
GO

INSERT INTO Categoria (Nombre) VALUES ('Comida');
INSERT INTO Categoria (Nombre) VALUES ('Bebida');
INSERT INTO Categoria (Nombre) VALUES ('Frituras');
GO

INSERT INTO Usuarios (
    Nombre, Apellido_Paterno, Apellido_Materno,
    Correo_Electronico, Contraseña, FechaNacimiento, tipo_UsuarioId
)
VALUES
('Juan', 'Pérez', 'Gómez', 'juan.perez@example.com', 'Password123', '1990-05-12', 1),
('María', 'López', 'Ramírez', 'maria.lopez@example.com', 'Password456', '1988-10-20', 2);
GO

---------------------------------------------------
-- Stored procedures
---------------------------------------------------

-- Validar si el producto ya está en el carrito
CREATE PROCEDURE ValidarProducto
    @idusuario INT,
    @idProducto UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 1
    FROM Carrito
    WHERE IdUsuarioId = @idusuario
      AND IdArticuloId = @idProducto;
END
GO

-- Agregar al carrito
CREATE PROCEDURE AgregarCarrito
    @idusuario INT,
    @idarticulo UNIQUEIDENTIFIER,
    @cantidad INT
AS
BEGIN
    INSERT INTO Carrito (IdUsuarioId, IdArticuloId, Cantidad)
    VALUES (@idusuario, @idarticulo, @cantidad);
END
GO

-- Eliminar
CREATE PROCEDURE EliminarCarrito
    @id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM Carrito WHERE Id = @id;
END
GO

-- Actualizar cantidad
CREATE PROCEDURE ActualizarCantidadCarrito
    @id UNIQUEIDENTIFIER,
    @cantidad INT
AS
BEGIN
    UPDATE Carrito
    SET Cantidad = @cantidad
    WHERE Id = @id;
END
GO

-- Obtener carrito del usuario
CREATE PROCEDURE ObtenerCarritoPorUsuario
    @idusuario INT
AS
BEGIN
    SELECT 
        c.Id,
        a.Nombre,
        a.Descripcion,
        a.Precio,
        c.Cantidad,
        (a.Precio * c.Cantidad) AS TotalLinea
    FROM Carrito c
    INNER JOIN Articulos a ON a.Id = c.IdArticuloId
    WHERE c.IdUsuarioId = @idusuario;
END
GO

-- Crear pedido
CREATE PROCEDURE CrearPedido
    @idusuario INT,
    @total DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idpedido UNIQUEIDENTIFIER = NEWID();

    INSERT INTO Pedidos (Id, ClienteId, EstatusId, Fecha, Total)
    VALUES (@idpedido, @idusuario, 1, GETDATE(), @total);

    INSERT INTO DetalleVenta (Id, IdPedidoId, IdProductoId, Cantidad, Precio)
    SELECT 
        NEWID(),
        @idpedido,         -- CORRECTO: IdPedido
        c.IdArticuloId,
        c.Cantidad,
        a.Precio
    FROM Carrito c
    INNER JOIN Articulos a ON a.Id = c.IdArticuloId
    WHERE c.IdUsuarioId = @idusuario;

    DELETE FROM Carrito WHERE IdUsuarioId = @idusuario;

    SELECT * FROM Pedidos WHERE Id = @idpedido;
END
GO

-- Validar correo
CREATE PROCEDURE ValidarCorreo
    @Correo NVARCHAR(255)
AS
BEGIN
    SELECT Correo_Electronico, Id
    FROM Usuarios
    WHERE Correo_Electronico = @Correo;
END
GO

-- Registrar usuario
CREATE PROCEDURE RegistroUsuario
    @Id INT,
    @Apellido_Paterno NVARCHAR(MAX),
    @Apellido_Materno NVARCHAR(MAX),
    @Nombre NVARCHAR(MAX),
    @Contraseña NVARCHAR(MAX),
    @TipoUsuario INT,
    @FechaRegistro DATETIME,
    @FechaNacimiento DATETIME,
    @Correo NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET IDENTITY_INSERT Usuarios ON;

    INSERT INTO Usuarios (Id, Nombre, Apellido_Paterno, Apellido_Materno, Correo_Electronico, Contraseña, FechaNacimiento, tipo_UsuarioId, FechaRegistro)
    VALUES (@Id, @Nombre, @Apellido_Paterno, @Apellido_Materno, @Correo, @Contraseña, @FechaNacimiento, @TipoUsuario, @FechaRegistro);

    SET IDENTITY_INSERT Usuarios OFF;
END
GO

-- Validar usuario
CREATE PROCEDURE ValidarUsuario
    @Id INT,
    @Contraseña NVARCHAR(MAX)
AS
BEGIN
    SELECT Id, tipo_UsuarioId
    FROM Usuarios
    WHERE Id = @Id AND Contraseña = @Contraseña;
END
GO

-- Validar token
CREATE PROCEDURE ValidarToken
    @Correo NVARCHAR(255)
AS
BEGIN
    SELECT Correo_Electronico, Id
    FROM Usuarios
    WHERE Correo_Electronico = @Correo;
END
GO

-- Cambiar contraseña
CREATE PROCEDURE CambiarContra
    @Id INT,
    @Contraseña NVARCHAR(MAX)
AS
BEGIN
    UPDATE Usuarios
    SET Contraseña = @Contraseña
    WHERE Id = @Id;
END
GO


select * from Articulos;

select * from Carrito;

select * from Pedidos;
