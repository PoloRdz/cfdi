USE [CFD19_GestionCFDi_V9999_R0]
GO
/****** Object:  StoredProcedure [dbo].[PG_IN_BITACORA_SQL]    Script Date: 6/1/2020 4:33:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[PG_UP_SALDO_FACTURA_INFO]
	@PP_L_DEBUG						[INT],
	@PP_K_SISTEMA_EXE				[INT],
	@PP_K_USUARIO					[INT],
	-- ===========================================
	@PP_ID_FACTURA					[INT] OUTPUT,
	@PP_SALDO						[DECIMAL] (18, 2)
AS
BEGIN
	
	UPDATE FACTURAS	
		SET SALDO = @PP_SALDO
	WHERE ID_FACTURA = @PP_ID_FACTURA

END
			
	-- ============================================
