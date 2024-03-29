USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  Table [dbo].[FACTURA]    Script Date: 5/29/2020 2:03:28 PM ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DESCRIPCION_PRODUCTOS](
	[PRODUCTO] [VARCHAR] (30) NOT NULL,
	[DESCRIPCION] [VARCHAR] (100) NOT NULL,
	[UNIDAD] [VARCHAR] (20) NOT NULL,
	[CLAVE_UNIDAD] [VARCHAR] (5) NOT NULL,
	[CLAVE_PROD_SERV] [VARCHAR] (15) NOT NULL,
	[NO_IDENTIFICACION] [VARCHAR] (15) NOT NULL
 CONSTRAINT [PK_DESCRIPCION_PRODUCTO] PRIMARY KEY CLUSTERED 
(
	[PRODUCTO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USO_CFDI](
	[USO_CFDI] [VARCHAR] (3) NOT NULL,
	[DESCRIPCION] [VARCHAR] (100) NOT NULL,
	[EXPLICACION] [VARCHAR] (200) NOT NULL,
	[FISICA] [BIT] NOT NULL,
	[MORAL] [BIT] NOT NULL,
	[L_USO_CFDI] [BIT] NOT NULL
 CONSTRAINT [PK_USO_CFDI] PRIMARY KEY CLUSTERED 
(
	[USO_CFDI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FACTURAS](
	[ID_FACTURA] [int] IDENTITY(1,1) NOT NULL,
	[FOLIO] [int] NOT NULL,
	--[ID_CLIENTE] [int] NULL,
	[K_RAZON_SOCIAL] [int] NOT NULL,
	[K_UNIDAD_OPERATIVA] [int] NOT NULL,
	[FOLIO_FISCAL] [varchar](50) NULL,
	[NOMBRE_RECEPTOR] [varchar] (250) NOT NULL,
	[RFC_RECEPTOR] [varchar] (13) NOT NULL,
	[EMAIL] [varchar] (255) NOT NULL,
	[TIPO_COMPROBANTE] [varchar] (3) NOT NULL,
	[USO_CFDI] [varchar] (3) NOT NULL,
	[SERIE] [varchar](10) NULL, --??
	[K_ESTATUS_FACTURA] [int] NOT NULL,
	[FECHA_CERTIFICACION] [datetime] NULL,
	[FECHA_EMISION] [datetime] NULL,
	[NO_CERTIFICADO_SAT] [varchar](50) NULL,
	[FORMA_PAGO] [varchar](50) NULL,
	[NO_CERTIFICADO_EMISOR] [varchar](50) NULL,
	[METODO_PAGO] [varchar](10) NULL,
	[SUB_TOTAL] [decimal](18, 2) NULL,
	[SUBTOTAL_IVA] [decimal](19,  6) NOT NULL,
	[TOTAL_IVA] [decimal](18, 2) NULL,
	[TOTAL] [decimal](18, 2) NULL,
	[SALDO] [DECIMAL](18, 2) NULL, 
	[IMPORTE_LETRA] [varchar](250) NULL,
	[CADENA_CERTIFICADO_DIGITAL_SAT] [varchar](2300) NULL,
	[SELLO_DIGITAL_EMISOR] [varchar](2300) NULL,
	[SELLO_DIGITAL_SAT] [varchar](2300) NULL,
	[RFC_PROV_CERTIF] [VARCHAR] (14) NULL,
	--[CANT] [int] NULL,
	[XML] [varchar] (MAX) NULL,
 CONSTRAINT [PK_FACTURA] PRIMARY KEY CLUSTERED 
(
	[ID_FACTURA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [FACTURAS]
	ADD CONSTRAINT FK_FACTURAS_RAZON_SOCIAL FOREIGN KEY (K_RAZON_SOCIAL)
		REFERENCES [RAZON_SOCIAL](K_RAZON_SOCIAL);
ALTER TABLE [FACTURAS]
	ADD CONSTRAINT FK_FACTURAS_UNIDAD_OPERATIVA FOREIGN KEY (K_UNIDAD_OPERATIVA)
		REFERENCES [UNIDAD_OPERATIVA](K_UNIDAD_OPERATIVA);
ALTER TABLE [FACTURAS]
	ADD CONSTRAINT FK_FACTURAS_USO_CFDI FOREIGN KEY ([USO_CFDI])
		REFERENCES [USO_CFDI]([USO_CFDI]);
ALTER TABLE [FACTURAS]
	ADD CONSTRAINT UQ_FACTURAS_FOLIO_SERIE UNIQUE ([FOLIO], [SERIE]);

/****** Object:  Table [dbo].[CONCEPTOS]    Script Date: 5/29/2020 2:03:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CONCEPTOS](
	[ID_CONCEPTO] [int] IDENTITY(1,1) NOT NULL,
	[ID_FACTURA] [int] NOT NULL,
	[ID_REMISION] [int] NOT NULL,
	[ID_LINKED_SERVER] [int] NOT NULL,
	[CLAVE_PROD_SERV] [varchar] (250) NOT NULL,
	[N_IDENTIFICACION] [varchar] (12) NOT NULL,
	[CLAVE_UNIDAD] [varchar] (5) NOT NULL,
	[UNIDAD] [varchar] (20) NOT NULL,
	[CANTIDAD] [decimal](18, 2) NULL,
	[U_MEDIDA] [varchar](50) NULL,
	[DESCRIPCION_PRODUCTO] [varchar](500) NULL,
	[P_UNITARIO] [decimal](18, 6) NULL,
	[IMPORTE] [decimal](18, 2) NULL,
	CONSTRAINT [PK_CONCEPTO] PRIMARY KEY CLUSTERED (
	[ID_CONCEPTO] ASC
)
) ON [PRIMARY]
GO
ALTER TABLE [CONCEPTOS]
	ADD CONSTRAINT FK_CONCEPTOS_FACTURA FOREIGN KEY (ID_FACTURA)
	REFERENCES [FACTURAS](ID_FACTURA);

ALTER TABLE [CONCEPTOS]
	ADD CONSTRAINT FK_CONCEPTOS_LINKED_SERVER FOREIGN KEY (ID_LINKED_SERVER)
	REFERENCES [LINKED_SERVERS](ID_LINKED_SERVER);

ALTER TABLE [CONCEPTOS]
	ADD CONSTRAINT UQ_CONCEPTOS_REMISIONES UNIQUE ([ID_REMISION], [ID_LINKED_SERVER]);

/****** Object:  Table [dbo].[CONCEPTOS]    Script Date: 5/29/2020 2:03:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMPUESTOS](
	[ID_IMPUESTO] [int] IDENTITY(1,1) NOT NULL,
	[ID_CONCEPTO] [int] NULL,
	[TIPO_IMPUESTO] [varchar] (10) NOT NULL,
	[BASE] [decimal] (18, 6) NOT NULL,
	[IMPUESTO] [varchar] (5) NOT NULL,
	[TASA_CUOTA] [decimal] (18, 6) NOT NULL,
	[TIPO_FACTOR] [varchar] (10) NOT NULL,
	[IMPORTE] [decimal] (18, 2) NOT NULL,
	CONSTRAINT [PK_IMPUESTO] PRIMARY KEY CLUSTERED 
(
	[ID_IMPUESTO] ASC
)
) ON [PRIMARY]
GO
ALTER TABLE [IMPUESTOS]
	ADD CONSTRAINT FK_IMPUESTOS_CONCEPTO FOREIGN KEY (ID_CONCEPTO)
	REFERENCES [CONCEPTOS](ID_CONCEPTO)
------------------------------------------------------------------
------------------------------------------------------------------
------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FACTURAS_RELACIONADAS](
	[ID_RELACION] [int] IDENTITY(1,1) NOT NULL,
	[ID_FACTURA] [int] NULL,
	[TIPO_RELACION] [varchar] (10) NOT NULL,
	[UUID] [varchar] (50) NOT NULL,
	CONSTRAINT [PK_RELACION] PRIMARY KEY CLUSTERED 
(
	[ID_RELACION] ASC
)
) ON [PRIMARY]
GO
ALTER TABLE [FACTURAS_RELACIONADAS]
	ADD CONSTRAINT FK_RELACIONES_FACTURAS FOREIGN KEY (ID_FACTURA)
		REFERENCES [FACTURAS](ID_FACTURA)


------------------------------------------------------------------
------------------------------------------------------------------
------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FOLIOS_DISPONIBLES](
	[ID_FOLIO_DISPONIBLE] [int] IDENTITY(1,1) NOT NULL,
	[K_RAZON_SOCIAL] [int] NULL,
	[FOLIOS] [int] NOT NULL,
	[FOLIOS_COMPARTIDOS] [int] NOT NULL
	CONSTRAINT [PK_FOLIOS_DISPONIBLES] PRIMARY KEY CLUSTERED ([ID_FOLIO_DISPONIBLE] ASC)
) ON [PRIMARY]
GO
ALTER TABLE [FOLIOS_DISPONIBLES]
	ADD CONSTRAINT FK_FOLIOS_RAZON_SOCIAL FOREIGN KEY (K_RAZON_SOCIAL)
		REFERENCES [RAZON_SOCIAL] (K_RAZON_SOCIAL);

ALTER TABLE [FOLIOS_DISPONIBLES]
	ADD CONSTRAINT UQ_FOLIOS_DISPONIBLES_RAZON_SOCIAL UNIQUE ([K_RAZON_SOCIAL]);

INSERT INTO [FOLIOS_DISPONIBLES] VALUES (84, 10000, 10000)
INSERT INTO [FOLIOS_DISPONIBLES] VALUES (13, 10000, 10000)
INSERT INTO [FOLIOS_DISPONIBLES] VALUES (42, 10000, 10000)
INSERT INTO [FOLIOS_DISPONIBLES] VALUES (36, 10000, 10000)
		 
------------------------------------------------------------------
------------------------------------------------------------------
------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DISTRIBUCION_FOLIOS](
	[ID_FOLIO_DISPONIBLE] [int] NOT NULL,
	[K_UNIDAD_OPERATIVA] [int] NOT NULL,
	[FOLIOS] [int] NOT NULL,
	[USAR_FOLIOS_COMPARTIDOS] [BIT] NOT NULL
	CONSTRAINT [PK_DISTRIBUCION_FOLIOS] PRIMARY KEY CLUSTERED ([ID_FOLIO_DISPONIBLE], [K_UNIDAD_OPERATIVA])
) ON [PRIMARY]
GO
ALTER TABLE [DISTRIBUCION_FOLIOS]
	ADD CONSTRAINT FK_DISTRIBUCION_FOLIOS_DISPONIBLES FOREIGN KEY ([ID_FOLIO_DISPONIBLE])
		REFERENCES [FOLIOS_DISPONIBLES] ([ID_FOLIO_DISPONIBLE]);

ALTER TABLE [DISTRIBUCION_FOLIOS]
	ADD CONSTRAINT FK_DISTRIBUCION_UNIDADES_OPERATIVAS FOREIGN KEY ([K_UNIDAD_OPERATIVA])
		REFERENCES [UNIDAD_OPERATIVA] ([K_UNIDAD_OPERATIVA]);
---------------------------------------------------------------
---------------------------------------------------------------
---------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SERIES](
	[ID_SERIE] [int] IDENTITY(1,1) NOT NULL,
	[K_RAZON_SOCIAL] [int] NULL,
	[TIPO_VENTA] [varchar] (10) NOT NULL,
	[SERIE] [varchar] (10) NOT NULL,
	[L_SERIE] [BIT] NOT NULL,
	CONSTRAINT [PK_RAZON_SOCIAL_SERIE] PRIMARY KEY CLUSTERED 
(
	[ID_SERIE] ASC
)
) ON [PRIMARY]
GO
ALTER TABLE [SERIES]
	ADD CONSTRAINT SERIES_RAZON_SOCIAL FOREIGN KEY (K_RAZON_SOCIAL)
	REFERENCES [RAZON_SOCIAL](K_RAZON_SOCIAL)

INSERT INTO SERIES VALUES (84, 'I', 'FPJ', 1)
INSERT INTO SERIES VALUES (84, 'E', 'FGPJ', 1)
INSERT INTO SERIES VALUES (13, 'I', 'FCB', 1)
INSERT INTO SERIES VALUES (13, 'E', 'FGCB', 1)
INSERT INTO SERIES VALUES (13, 'I', 'FCR', 1)
INSERT INTO SERIES VALUES (13, 'P', 'FPR', 1)


---------------------------------------------------------------
---------------------------------------------------------------
---------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAGOS](
	[ID_PAGO] [int] IDENTITY(1,1) NOT NULL,
	[ID_FACTURA] [INT] NOT NULL,
	[FECHA_PAGO] [DATETIME],
	[FORMA_PAGO] [VARCHAR] (3) NOT NULL,
	[MONEDA] [VARCHAR] (5) NOT NULL,
	[MONTO] [DECIMAL] (19, 2) NOT NULL,
	[NUM_OPERACION] [INT],
	[CUENTA_BENEFICIARIO] [VARCHAR] (30) NOT NULL,
	[CUENTA_ORDENANTE] [VARCHAR] (30) NOT NULL
	CONSTRAINT [PK_PAGOS] PRIMARY KEY CLUSTERED ([ID_PAGO] ASC)
) ON [PRIMARY]
GO
ALTER TABLE [PAGOS]
	ADD CONSTRAINT FK_PAGOS_FACTURA FOREIGN KEY (ID_FACTURA)
		REFERENCES [FACTURAS] (ID_FACTURA)


---------------------------------------------------------------
---------------------------------------------------------------
---------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DOCUMENTOS_RELACIONADOS](
	[ID_DOCUMENTO_RELACIONADO] [int] IDENTITY(1,1) NOT NULL,
	[ID_PAGO] [INT] NOT NULL,
	[ID_FACTURA] [INT]  NOT NULL,
	[MONEDA_DR] [VARCHAR] (5) NOT NULL,
	[METODO_PAGO] [VARCHAR] (5) NOT NULL,
	[NUMERO_PARCIALIDAD] [INT] NOT NULL,
	[SALDO_ANTERIOR] [DECIMAL] (19, 2) NOT NULL,
	[IMPORTE_PAGADO] [DECIMAL] (19, 2) NOT NULL,
	[SALDO_INSOLUTO] [DECIMAL] (19, 2) NOT NULL,
	CONSTRAINT [PK_DOCUMENTOS_RELACIONADOS] PRIMARY KEY CLUSTERED ([ID_DOCUMENTO_RELACIONADO] ASC)
) ON [PRIMARY]
GO
ALTER TABLE [DOCUMENTOS_RELACIONADOS]
	ADD CONSTRAINT FK_PAGOS_DOCUMENTOS_RELACIONADOS FOREIGN KEY (ID_PAGO)
		REFERENCES [PAGOS] (ID_PAGO)

ALTER TABLE [DOCUMENTOS_RELACIONADOS]
	ADD CONSTRAINT FK_FACTURA_DOCUMENTOS_RELACIONADOS FOREIGN KEY (ID_FACTURA)
		REFERENCES [FACTURAS] (ID_FACTURA)


---------------------------------------------------------------
---------------------------------------------------------------
---------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIOS](
	[ID_USUARIO] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE_USUARIO] [VARCHAR] (15) NOT NULL,
	[CORREO] [VARCHAR] (100) NOT NULL,
	[CONTRASENA] [VARCHAR] (256) NOT NULL,
	[NOMBRE] [VARCHAR] (75) NOT NULL,
	[APELLIDO_P] [VARCHAR] (30) NOT NULL,
	[APELLIDO_M] [VARCHAR] (30) NOT NULL,
	[L_USUARIO] [BIT] NOT NULL
	CONSTRAINT [PK_USUARIOS] PRIMARY KEY CLUSTERED ([ID_USUARIO] ASC)
) ON [PRIMARY]
GO

ALTER TABLE [USUARIOS]
	ADD CONSTRAINT UQ_USUARIOS_NOMBRE_USUARIO UNIQUE ([NOMBRE_USUARIO]);
ALTER TABLE [USUARIOS]
	ADD CONSTRAINT UQ_USUARIOS_CORREO UNIQUE ([CORREO]);

INSERT INTO USUARIOS (NOMBRE_USUARIO, CORREO, CONTRASENA, NOMBRE, APELLIDO_P, APELLIDO_M, L_USUARIO) VALUES ('hrodriguez', 'hrodriguez.a@tomza.com', '$HASH|V1$10000$PpXsytvIV0qEJ8FfHy+88kXIsFocAlTRDx9+H6lUku92zh58', 'Hipólito', 'Rodríguez', 'Alvarado', 1);
INSERT INTO USUARIOS (NOMBRE_USUARIO, CORREO, CONTRASENA, NOMBRE, APELLIDO_P, APELLIDO_M, L_USUARIO) VALUES ('csanchez', 'csanchez.g@tomza.com', '$HASH|V1$10000$ZIzm2bjdzlpmHjqH1+DzCoLhq5cIxAJyAecmYfjEUP6WBL/E', 'Carlos Alejandro', 'Sánchez', 'García', 1);
--INSERT INTO USUARIOS (NOMBRE_USUARIO, CORREO, CONTRASENA, NOMBRE, APELLIDO_P, APELLIDO_M, L_USUARIO) VALUES ('hrodriguez', 'hrodriguez.a@tomza.com', '', 'Hipólito', 'Rodríguez', 'Alvarado', 1)
--INSERT INTO USUARIOS (NOMBRE_USUARIO, CORREO, CONTRASENA, NOMBRE, APELLIDO_P, APELLIDO_M, L_USUARIO) VALUES ('hrodriguez', 'hrodriguez.a@tomza.com', '', 'Hipólito', 'Rodríguez', 'Alvarado', 1)


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[REGIMENES_FISCALES](
	[ID_REGIMEN_FISCAL] [int] NOT NULL,
	[DESCRIPCION] [VARCHAR] (100) NOT NULL,
	[FISICA] [BIT] NOT NULL,
	[MORAL] [BIT] NOT NULL,
	[L_REGIMEN_FISCAL] [BIT] NOT NULL
 CONSTRAINT [PK_REGIMEN_FISCAL] PRIMARY KEY CLUSTERED 
(
	[ID_REGIMEN_FISCAL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO USO_CFDI (USO_CFDI, DESCRIPCION, EXPLICACION, FISICA, MORAL, L_USO_CFDI)
VALUES  ('G01', 'Adquisición de Mercancías', 'Compras necesarias para vender', 1, 1, 1),
		('G02', 'Devoluciones, Descuentos o Bonificaciones', 'Conocido como Notas de Crédito, cuando se descuenta un monto del CFDI original', 1, 1, 1),
		('G03', 'Gastos en General', 'Cuando el concepto no engloba en los gastos predefinidos.', 1, 1, 1),
		('I01', 'Construcciones', 'Incluye ampliaciones y mejoras.', 1, 1, 1),
		('I02', 'Mobiliario y Equipo de Oficina por Inversiones', 'Compra de muebles o equipo de oficina.', 1, 1, 1),
		('I03', 'Equipo de Transporte', 'Automóviles, camionetas o cualquier vehículo automotor.', 1, 1, 1),
		('I04', 'Equipo de Cómputo y Accesorios', 'Computadoras y accesorios como impresoras, escaners, etc', 1, 1, 1),
		('I05', 'Dados, Troqueles, Moldes, Matrices y Herramental', 'Herramientas necesarias para la producción.', 1, 1, 1),
		('I06', 'Comunicaciones Telefónicas', 'Telefonía fija, celular e Internet.', 1, 1, 1),
		('I07', 'Comunicaciones Satelitales', 'Telefonía o Internet satelital.', 1, 1, 1),
		('I08', 'Otra Maquinaria y Equipo', 'Equipo que será empleado en la producción y no esta englobado en las otras categorías.', 1, 1, 1),
		('D01', 'Honorarios Médicos, Dentales y Gastos Hospitalarios', 'Recibos de Honorarios por consultas o intervenciones.', 1, 1, 1),
		('D02', 'Gastos Médicos por Incapacidad o Discapacidad', 'En caso de accidente, es la compra de sillas de ruedas, muletas, etc.', 1, 1, 1),
		('D03', 'Gastos Funerales', 'Pagos por muerte de cónyuge o dependiente económico.', 1, 1, 1),
		('D04', 'Donativos', 'Para Asociaciones Civiles o fundaciones.', 1, 1, 1),
		('D05', 'Intereses Reales Efectivamente Pagados por Créditos Hipotecarios (Casa Habitación)', 'Pagos de Hipotecas', 1, 1, 1),
		('D06', 'Aportaciones Voluntarias al SAR', 'Aportaciones voluntarias al fondo ahorro para el retiro, que no estén incluida en el CFDI de Nómina.', 1, 1, 1),
		('D07', 'Primas por Seguros de Gastos Médicos', 'En la compra de un seguro de gastos médicos.', 1, 1, 1),
		('D08', 'Gastos de Transportación Escolar Obligatoria', 'Solo si las instituciones obligan al pago y emiten comprobante CFDI.', 1, 1, 1),
		('D09', 'Depósitos en Cuentas para el Ahorro, Primas que tengan como Base Planes de Pensiones', 'Aportaciones a otros planes para el retiro.', 1, 1, 1),
		('D10', 'Pagos por Servicios Educativos (Colegiaturas)', 'Pago de colegiaturas de Pre-Escolar, hasta Preparatoria.', 1, 1, 1),
		('P01', 'Por Definir', 'Rubros que no se encuentran definidos en el catálogo de uso.', 1, 1, 1)

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[INFORMACION_FISCAL_USUARIOS](
	[ID_INFORMACION_FISCAL] [int] IDENTITY(1,1) NOT NULL,
	[ID_USUARIO] [INT] NOT NULL,
	[RFC] [VARCHAR] (13) NOT NULL,
	[RAZON_SOCIAL] [VARCHAR] (100)NOT NULL,
	[DIRECCION_FISCAL] [VARCHAR] (200) NOT NULL,
	[CODIGO_POSTAL] [VARCHAR] (10) NOT NULL,
	[TELEFONO] [VARCHAR] (20) NULL,
	[ID_REGIMEN_FISCAL] [INT] NOT NULL
 CONSTRAINT [PK_INFORMACION_FISCAL] PRIMARY KEY CLUSTERED 
(
	[ID_INFORMACION_FISCAL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [INFORMACION_FISCAL_USUARIOS]
	ADD CONSTRAINT FK_INFORMACION_FISCAL_REGIMEN_FISCAL FOREIGN KEY ([ID_REGIMEN_FISCAL])
		REFERENCES [REGIMENES_FISCALES]([ID_REGIMEN_FISCAL]);
ALTER TABLE [INFORMACION_FISCAL_USUARIOS]
	ADD CONSTRAINT FK_INFORMACION_FISCAL_USUARIO FOREIGN KEY ([ID_USUARIO])
		REFERENCES [USUARIOS]([ID_USUARIO]);
ALTER TABLE [INFORMACION_FISCAL_USUARIOS]
	ADD CONSTRAINT UQ_INFORMACION_FISCAL_RFC UNIQUE ([RFC]);


/****** Object:  Table [dbo].[LINKED_SERVER]    Script Date: 7/4/2020 10:26:44 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LINKED_SERVERS](
	[ID_LINKED_SERVER] [int] IDENTITY (1,1) NOT NULL,
	[K_UNIDAD_OPERATIVA] [int] NOT NULL,
	[D_LINKED_SERVER] [varchar](100) NOT NULL,
	[C_LINKED_SERVER] [varchar](255) NOT NULL,
	[S_LINKED_SERVER] [varchar](10) NOT NULL,
	[K_TIPO_LINKED_SERVER] [int] NOT NULL,
	[O_LINKED_SERVER] [int] NOT NULL,
	[L_LINKED_SERVER] [int] NOT NULL,
 CONSTRAINT [PK_LINKED_SERVER] PRIMARY KEY CLUSTERED 
(
	[ID_LINKED_SERVER] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [LINKED_SERVERS]
	ADD CONSTRAINT [FK_LINKED_SERVER_UNIDAD_OPERATIVA] FOREIGN KEY ([K_UNIDAD_OPERATIVA])
		REFERENCES [UNIDAD_OPERATIVA] ([K_UNIDAD_OPERATIVA]);


-------------------------------------------------------------
-------------------------------------------------------------
-------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RELACION_IMPUESTOS_UO](
	[ID_RELACION_IMP_UO] [int] IDENTITY(1,1) NOT NULL,
	[K_UNIDAD_OPERATIVA] [int] NOT NULL,
	[IMPUESTOS] [DECIMAL] (8, 6) NOT NULL,
	[FECHA_EXPIRACION] [DATE] NOT NULL,
	CONSTRAINT [PK_RELACION_IMPUESTOS_UO] PRIMARY KEY CLUSTERED ([ID_RELACION_IMP_UO] ASC)
) ON [PRIMARY]
GO
ALTER TABLE [RELACION_IMPUESTOS_UO]
	ADD CONSTRAINT FK_RELACION_IMPUESTOS_UO FOREIGN KEY ([K_UNIDAD_OPERATIVA])
		REFERENCES [UNIDAD_OPERATIVA] ([K_UNIDAD_OPERATIVA])

-------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLES](
	[ID_ROL] [int] IDENTITY(1,1) NOT NULL,
	[ROL] [VARCHAR](20) NOT NULL,
	[IDENTIFICADOR] [VARCHAR] (50) NOT NULL,
	[DESCRIPCION] [VARCHAR] (100) NOT NULL,
	CONSTRAINT [PK_ROLES] PRIMARY KEY CLUSTERED ([ID_ROL] ASC)
) ON [PRIMARY]
GO


-------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIOS_ROLES](
	[ID_ROL] [int] NOT NULL,
	[ID_USUARIO] [INT] NOT NULL,
	CONSTRAINT [PK_USUARIOS_ROLES] PRIMARY KEY CLUSTERED ([ID_ROL], [ID_USUARIO] ASC)
) ON [PRIMARY]
GO
ALTER TABLE [USUARIOS_ROLES]
	ADD CONSTRAINT FK_USUARIOS_ROLES_ROL FOREIGN KEY ([ID_ROL])
		REFERENCES [ROLES] ([ID_ROL])
ALTER TABLE [USUARIOS_ROLES]
	ADD CONSTRAINT FK_USUARIOS_ROLES_USUARIO FOREIGN KEY ([ID_USUARIO])
		REFERENCES [USUARIOS] ([ID_USUARIO])

-------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------

INSERT INTO REGIMENES_FISCALES ([ID_REGIMEN_FISCAL], [DESCRIPCION], [FISICA], [MORAL], [L_REGIMEN_FISCAL])
VALUES (601, 'General de Ley Personas Morales', 0, 1, 1),
	   (603, 'Personas Morales con Fines no Lucrativos', 0, 1, 1),
	   (605, 'Sueldos y Salarios e Ingresos Asimilados a Salarios', 1, 0, 1),
	   (606, 'Arrendamiento', 1, 0, 1),
	   (608, 'Demás ingresos', 1, 0, 1),
	   (609, 'Consolidación', 0, 1, 1),
	   (610, 'Residentes en el Extranjero sin Establecimiento Permanente en México', 1, 1, 1),
	   (611, 'Ingresos por Dividendos (socios y accionistas)', 1, 0, 1),
	   (612, 'Personas Físicas con Actividades Empresariales y Profesionales', 1, 0, 1),
	   (614, 'Ingresos por intereses', 1, 0, 1),
	   (616, 'Sin obligaciones fiscales', 1, 0, 1),
	   (620, 'Sociedades Cooperativas de Producción que optan por diferir sus ingresos', 0, 1, 1),
	   (621, 'Incorporación Fiscal', 1, 0, 1),
	   (622, 'Actividades Agrícolas, Ganaderas, Silvícolas y Pesqueras', 1, 1, 1),
	   (623, 'Opcional para Grupos de Sociedades', 0, 1, 1),
	   (624, 'Coordinados', 0, 1, 1),
	   (628, 'Hidrocarburos', 0, 1, 1),
	   (607, 'Régimen de Enajenación o Adquisición de Bienes', 0, 1, 1),
	   (629, 'De los Regímenes Fiscales Preferentes y de las Empresas Multinacionales', 1, 0, 1),
	   (630, 'Enajenación de acciones en bolsa de valores', 1, 0, 1),
	   (615, 'Régimen de los ingresos por obtención de premios', 1, 0, 1)

INSERT INTO LINKED_SERVERS (K_UNIDAD_OPERATIVA, D_LINKED_SERVER, C_LINKED_SERVER, S_LINKED_SERVER, K_TIPO_LINKED_SERVER, O_LINKED_SERVER, L_LINKED_SERVER)
VALUES
	(31, 'ACAPULCO', 'LINKED_SERVER_ACAPULCO', 'ACA', 1, 0, 1),
	(38, 'AGUAPRIETA', 'LINKED_SERVER_AGUAPRIETA', 'AGU', 1, 0, 1),
	(68, 'AGUASCALIENTES', 'LINKED_SERVER_AGUASCALIENTES', 'AGU', 1, 0, 1),
	(57, 'AUTLAN', 'LINKED_SERVER_AUTLAN', 'AUT', 1, 0, 1),
	(16, 'BENITOJUAREZ', 'LINKED_SERVER_BENITOJUAREZ', 'BEN', 1, 0, 1),
	(3, 'BIOGAS', 'LINKED_SERVER_BIOGAS', 'BIO', 1, 0, 1),
	(44, 'CABORCA', 'LINKED_SERVER_CABORCA', 'CAB', 1, 0, 1),
	(59, 'CANCUN', 'LINKED_SERVER_CANCUN', 'CAN', 1, 0,	1),
	(29, 'GASURIBE', 'LINKED_SERVER_GASURIBE', 'GAS', 1, 0, 1),
	(80, 'GUANAJUATO', 'LINKED_SERVER_GUANAJUATO', 'GUA', 1, 0, 1),
	(46, 'GUAYMAS', 'LINKED_SERVER_(!) GUAYMAS', '(!)', 1, 0, 1),
	(72, 'HIDALGO', 'LINKED_SERVER_(X) HIDALGO', '(X)', 1, 0, 1),
	(7, 'HIDROI', 'LINKED_SERVER_(X) HIDROI', '(X)', 1, 0, 1),
	(51, 'LALAJA', 'LINKED_SERVER_LALAJA', 'LAL', 1, 0, 1),
	(4, 'LASSIERRA', 'LINKED_SERVER_LASSIERRA', 'LAS', 1, 0, 1),
	(70, 'MAMEMEGAS', 'LINKED_SERVER_MAMEMEGAS', 'MAM', 1, 0, 1),
	(84, 'MATRIZ', 'LINKED_SERVER_MATRIZ', 'MAT', 1, 0, 1),
	(62, 'MERIDA', 'LINKED_SERVER_MERIDA', 'MER', 1, 0, 1),
	(66,	'MINATITLAN',	'LINKED_SERVER_(X) MINATITLAN',	'(X)',	1,	0,	1),
	(73,	'MOCHIS',	'LINKED_SERVER_MOCHIS',	'MOC',	1,	0,	1),
	(32,	'MORELOS',	'LINKED_SERVER_MORELOS',	'MOR',	1,	0,	1),
	(40,	'NACO',	'LINKED_SERVER_NACO',	'NAC',	1,	0,	1),
	(41,	'NACOZARI',	'LINKED_SERVER_NACOZARI',	'NAC',	1,	0,	1),
	(48, 	'NAVOJOA',	'LINKED_SERVER_(!) NAVOJOA',	'(!)',	1,	0,	1),
	(42,	'NOGALES',	'LINKED_SERVER_NOGALES',	'NOG',	1,	0,	1),
	(47,	'OBREGON',	'LINKED_SERVER_OBREGON',	'OBR',	1,	0,	1),
	(54,	'OCOTLAN',	'LINKED_SERVER_OCOTLAN',	'OCO',	1,	0,	1),
	(12,	'OJINAGA',	'LINKED_SERVER_(X) OJINAGA',	'(X)',	1,	0,	1),
	(5,	'PALOMAS',	'LINKED_SERVER_PALOMAS',	'PAL',	1,	0,	1),
	(2,	'PARRAL',	'LINKED_SERVER_(X) PARRAL',	'(X)',	1,	0,	1),
	(78,	'PEPSICO',	'LINKED_SERVER_PEPSICO',	'PEP',	1,	0,	1),
	(71,	'PLAYADELCARMEN', 	'LINKED_SERVER_PLAYADELCARMEN', 	'PLA',	1,	0,	1),
	(6,	'PUEBLO',	'LINKED_SERVER_PUEBLO',	'PUE',	1,	0,	1),
	(45,	'PUERTOPENASCO',	'LINKED_SERVER_PUERTOPENASCO',	'PUE',	1,	0,	1),
	(28,	'QUERETARO',	'LINKED_SERVER_QUERETARO',	'QUE',	1,	0,	1),
	(27,	'SANLUISPOTOSI',	'LINKED_SERVER_(X) SANLUISPOTOSI',	'(X)',	1,	0,	1),
	(49,	'SANLUISRIOCOLORADO',	'LINKED_SERVER_SANLUISRIOCOLORADO',	'SAN',	1,	0,	1),
	(69,	'SANQUINTIN',	'LINKED_SERVER_SANQUINTIN',	'SAN',	1,	0,	1),
	(43,	'SANTAANA',	'LINKED_SERVER_(X) SANTAANA',	'(X)',	1,	0,	1),
	(26,	'TEHUACAN',	'LINKED_SERVER_TEHUACAN',	'TEH',	1,	0,	1),
	(53,	'TEPATITLAN',	'LINKED_SERVER_TEPATITLAN',	'TEP',	1,	0,	1),
	(77,	'TESISTAN',	'LINKED_SERVER_TESISTAN',	'TES',	1,	0,	1),
	(63,	'TICUL',	'LINKED_SERVER_TICUL',	'TIC',	1,	0,	1),
	(33,	'TIJUANA',	'LINKED_SERVER_TIJUANA',	'TIJ',	1,	0,	1),
	(24,	'TLAHUAC',	'LINKED_SERVER_TLAHUAC',	'TLA',	1,	0,	1),
	(25,	'TOLUCA',	'LINKED_SERVER_TOLUCA',	'TOL',	1,	0,	1),
	(23,	'TOMZAMEXICANA',	'LINKED_SERVER_TOMZAMEXICANA',	'TOM',	1,	0,	1),
	(37,	'URES',	'LINKED_SERVER_(X) URES',	'(X)',	1,	0,	1),
	(79,	'VERACRUZ',	'LINKED_SERVER_(X) VERACRUZ',	'(X)',	1,	0,	1),
	(55,	'YAHUALICA',	'LINKED_SERVER_YAHUALICA',	'YAH',	1,	0,	1),
	(30,	'ZAPOPAN',	'LINKED_SERVER_ZAPOPAN',	'ZAP',	1,	0,	1),
	(29,	'GAS_TOMZA_PLUEBLA', '	LINKED_SERVER_GAS_TOMZA_PLUEBLA', '	GAS',	1,	0,	1),
	(29,	'MEXICANAGAS', '	LINKED_SERVER_MEXICANAGAS', '	GAS',	1,	0,	1),
	(29,	'TEPEJI', '	LINKED_SERVER_TEPEJI', '	GAS',	1,	0,	1),
	(29,	'UNIGASSANJOSE', '	LINKED_SERVER_UNIGASSANJOSE', '	GAS',	1,	0,	1),
	(29,	'ICS_Transportadora', '	LINKED_SERVER_ransportadora', '	GAS',	1,	0,	1),
	(29,	'ICS_GasyPetroleos',	'LINKED_SERVER_GasyPetroleos', '	GAS',	1,	0,	1);


INSERT INTO FISCALES_EMISOR(K_FISCALES_EMISOR, D_FISCALES_EMISOR, C_FISCALES_EMISOR, L_FISCALES_EMISOR, K_UNIDAD_OPERATIVA, K_RAZON_SOCIAL, K_CERTIFICADO, REGIMEN_FISCAL, F_ALTA, K_ALTA_USUARIO, F_CAMBIO,K_CAMBIO_USUARIO)
VALUES (3, 'GAS COMERCIAL DE VILLA AHUMADA SA DE CV', 'GAS COMERCIAL DE VILLA AHUMADA SA DE CV', 1, 3, 13, 7, 601, GETDATE(), 44, GETDATE(), 44)

INSERT INTO [RELACION_IMPUESTOS_UO] (K_UNIDAD_OPERATIVA, IMPUESTOS, FECHA_EXPIRACION)
VALUES (3, 0.080000, '2020/12/31')	

INSERT INTO [INFORMACION_FISCAL_USUARIOS] (ID_USUARIO, RFC, RAZON_SOCIAL, DIRECCION_FISCAL, CODIGO_POSTAL, TELEFONO, ID_REGIMEN_FISCAL)
VALUES (1, 'XAXX010101000', 'VENTA PUBLICO GENERAL', 'DIRECICON FISCAL JUAREZ', '32000', '6567894561', 605)


INSERT INTO DESCRIPCION_PRODUCTOS (PRODUCTO, DESCRIPCION, UNIDAD, CLAVE_UNIDAD, CLAVE_PROD_SERV, NO_IDENTIFICACION)
VALUES ('LITRO', 'GAS LICUADO', 'LITRO', 'LTR', '1111111111', '11111'),
	   ('AUTOSONSUMO', 'AUTOCONSUMO', '??', '', '1111111112', '11112'),
	   ('BONIFICACION', 'BONIFICACION', '', '', '1111111113', '11113'),
	   ('CIL 45 KGS', 'CIL 45 KGS', '', '', '1111111114', '11114'),
	   ('CIL 30 KGS', 'CIL 30 KGS', '', '', '1111111115', '11115'),
	   ('CIL 10 KGS', 'CIL 10 KGS', '', '', '1111111116', '11116'),
	   ('KILOGRAMO', '', 'KILOGRAMO', 'KG', '1111111117', '11117'),
	   ('COMPENSACION', 'COMPENSACION', '', '', '1111111118', '11118'),
	   ('DESCUENTO', 'DESCUENTO', '', '', '1111111119', '11119')


INSERT INTO ROLES (ROL, IDENTIFICADOR, DESCRIPCION) 
VALUES ('Administrador', 'ADMIN', 'Administrador del sistema'),
	   ('Básico', 'BASIC', 'Usuario por defecto del sistema'),
	   ('Folios', 'FOLIO', 'Rol para manejar Folios'),
	   ('Certificados', 'CERTS', 'Rol para manejar Certificados'),
	   ('Serie', 'SERIE', 'Rol para manejar Series'),
	   ('Razón Social', 'RZS', 'Rol para manejar Razón Social'),
	   ('Zona', 'ZONA', 'Rol para manejar Zona'),
	   ('Unidad Operativa', 'UOP', 'Rol para manejar Unidad Operativa'),
	   ('UsoCFDI', 'UCFDI', 'Rol para manejar UsoCFDI'),
	   ('Regimen Fiscal', 'REGFI', 'Rol para manejar Regimen Fiscal')
