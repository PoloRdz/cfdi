USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[PG_SK_CONCEPTOS_FACTURA]
	@PP_ID_FACTURA			INT
AS

	BEGIN
		SELECT 
			C.ID_REMISION,
			C.ID_LINKED_SERVER,
			C.UNIDAD,
			C.CLAVE_UNIDAD,
			C.DESCRIPCION_PRODUCTO,
			C.CLAVE_PROD_SERV,
			C.N_IDENTIFICACION,
			C.CANTIDAD,
			C.P_UNITARIO,
			C.IMPORTE,
			0.0 AS DESCUENTO
		FROM CONCEPTOS C
			INNER JOIN FACTURAS F
				ON C.ID_FACTURA = F.ID_FACTURA
		WHERE F.ID_FACTURA = @PP_ID_FACTURA
	END
		

	-- ===============================

