USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_SK_CERT_INFO_EMISOR]    Script Date: 6/1/2020 10:29:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[PG_SK_CERT_XML_INFO]
	@PP_L_DEBUG					INT,
	@PP_K_SISTEMA_EXE			INT,
	@PP_ID_FOLIO				INT
AS

	SELECT 
		F.XML
	FROM FACTURAS F
	WHERE 
		ID_FACTURA = @PP_ID_FOLIO
		

	-- ===============================

