USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[PG_RN_FACTURA_STATUS]
	@PP_L_DEBUG					INT,
	@PP_K_SISTEMA_EXE			INT,
	@PP_SERIE					VARCHAR(10),
	@PP_FOLIO					INT,
	@PP_STATUS_CODE				INT OUTPUT
AS
BEGIN
	DECLARE @VP_STATUS_ID INT = -1

	SELECT @VP_STATUS_ID = F.K_ESTATUS_FACTURA
	FROM FACTURAS F
		WHERE 
			F.SERIE = @PP_SERIE AND
			F.FOLIO = @PP_FOLIO

	IF @VP_STATUS_ID IN (1, 2, 3, 4, 5)
		SET @PP_STATUS_CODE = 1
	IF @VP_STATUS_ID IN (0, 6)
		SET @PP_STATUS_CODE = 0
		
END	
	-- ===============================

