Welcome to iFruit - Photo editing & organizer can't do miracles

----------------INDICE----------------
-Antes de usar(hablar sobre la api key)
-Import to my library
-Export to my library
-Editing Area
-Pasos para modificar y guardar la imagen
-Manage Library
-Search in my library
-Manage Smart list
-Exit




----------------ANTES DE USAR----------------

-El programa usa un sistema de api key para funcionar, el cual por Default no esta en el codigo por temas de posible hurto, debido a eso la secuencia alfanumerica
se encuentra en un archivo zip dentro del repositorio.
En la carpeta principal del repositorio se encuentra un zip llamado "ClaveToInsert", dentro de el se encuentra un txt llamado "ClaveToInsert", se copia la clave del txt y se pega
en la linea 13 de la clase "WatsonAnalizer".
Ya con esto hecho puede  ejecutar su programa.




----------------IMPORT TO MY LIBRARY----------------

Al seleccionar import to my library se le pide al usuario ingresar la direccion donde se encuentra la imagen que desea importar, seguido de un "\". 
Ejemplo: "C:\Users\torre\Pictures\ax\"

Seguidamente se le pide al usuario ingresar el nombre de la imagen a seleccionar, seguido de su formato de archivo
Ejemplo: foto1.jpg

Si el usuario quiere ingresar una carpeta completa de fotos, pone en nombre de imagen "all", al momento de ingresar etiquetas, se le preguntara al usuario si quiere etiquetar todas de la misma
manera o cada una por separado.


Cuando la  imagen haya sido seleccionada se le pedira al usuario si es que quiere ingresar una calificacion(1-5) y/o label
Recordatorio: Simple Label consiste de un tag, recomendado por Watson o ingresado por el usuario.
	      Person Label le pide al usuario ingresar un nombre,apellido,localizacion de la cara,nacionalidad,color de ojos,color de pelo,sexo,fecha de nacimiento (todos datos son opcionales)
	      Special Label le pide al usuario ingresar una locacion geografica,direccion,fotografo que tomo la foto,motivo de la foto,si es selfie o no (todos los datos son opcionales)

Con todo esto se crea una imagen que queda guardada en libreria.




----------------EXPORT TO MY LIBRARY----------------

Al seleccionar export to my library se le presenta al usuario el listado de foto dentro de my library, el usuario selecciona 1  y se le pide ingresar la direccion donde se desea guardar, segido de un "\"
Ejemplo: "C:\Users\torre\Pictures\ax\"

Luego se le pide al usuario que ingrese el nombre que desea para su archivo

Y por ultimo se le da a elegir 4 formatos de imagenes(jpg,jpeg,bmp,png)

La foto se exporta a la direccion dada con el nombre y formado dado.




----------------EDITING AREA----------------

INDICACION: Antes de usar el apply filters o use features debe por lo menos tener una imagen importada en editing area.

Al seleccionar Editing Area, se le presenta al usuario otro menu con diversas opciones:


	APPLY FILTERS  --> Menu de seleccion de filtros(denotados mas abajo en el documento)
	USE FEATURES  --> Menu de seleccion de herramientas para modificar la imagen(denotados mas abajo en el documento)
	IMPORT IMAGES TO THE EDITING AREA: Se le presenta al usuario un listado de imagenes que tiene en my library, el usuario debe seleccionar una o mas fotos que desea importar. 
	Para confirmar sus imagenes debe seleccionar continuar.	
	DELETE IMAGES FROM THE EDITING AREA : Se le presenta al usuario un listado de imagenes que tiene en editing area, el usuario debe seleccionar una o mas fotos que desea eliminar. 
	Para confirmar sus imagenes debe seleccionar continuar.	
	EXPORT IMAGES FROM THE EDITING AREA: Se le presenta al usuario un listado de imagenes que tiene en editing area, el usuario debe seleccionar una o mas fotos que desea exportar. 
	Para confirmar sus imagenes debe seleccionar continuar.	Si es que la imagen que se exporta tiene el mismo  nombre y dimensiones que una imagen en my library se le pedira al usuario
	si es que quiere reemplazar el archivo, en el caso de que no, se le pide al usuario si quiere o no cambiar el nombre.Se creara una nueva imagen en my library.

LISTA DE FILTROS
	BlackNWhiteFilter: Este filtro retorna la imagen con solo colores blanco y negro.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.
	BrightnessFilter: Este filtro retorna la imagen con un brillo mas intenso.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.
	ColorFilter: Este filtro retorna la imagen con el color elegido.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.
	Se muestra un menu con las opciones de Select a color from list o Select a custom color. En esta ultima tiene que ingresar los datos como formato R,G,B(separando por ,).
	InvertFiler: Este filtro retorna la imagen con los colores invertidos.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.
	MirrorFilter: Este filtro retorna la imagen en modo espejo.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.
	OldFilmFilter: Este filtro retorna la imagen con colores grises y puntos blancos.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.
	RotateFlipFilter: Este filtro retorna la imagen rotada segun el angulo que el usuario elija.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.
	Finalmente, se le muestra al usuario un menu con las opciones que tiene para rotar la imagen.
	SepiaFilter: Este filtro retorna la imagen con colores marrones.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.
	WindowsFilter: Este filtro retorna la imagen con cuatro filtros de colores, formando  el logo de Windows.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.
	AutomaticAdjustmentFilter: Este filtro retorna la imagen con colores mas intensos.Se le pide al usuario que seleccione una o mas fotos del editing area y luego debe confirmar presionando continue.

LISTA DE  FEATURES

	ADD CENSORSHIP: Se le presenta al usuario un listado de imagenes que tiene en editing area, el usuario debe seleccionar una o mas fotos a censurar. Luego elige el tipo de censura (barra negra o pixelasion). Po ultimo el ususario ingresa el tamaño del area censurar ademas de las cordenadas.
	WATSON FACE RECOGNITION ANALIZER: Feature still in progress
	ADD TEXT: Se le presenta al usuario un listado de imagenes que tiene en editing area, el usuario debe seleccionar una o mas fotos a las que quiere agregar el texto. Luego el usuario debe ingresar el texto que desea poner en sus imagenes al igual que las coordenadas de donde cituar el texto, el tamaño de la fuente, el nombre de la fuente y los colores del texto.
	MERGE IMAGES: Se le presenta al usuario un listado de imagenes que tiene en editing area, el usuario debe seleccionar dos o mas fotos que quiere merger.
	RESIZE IMAGE: Se le presenta al usuario un listado de imagenes que tiene en editing area, el usuario debe seleccionar una o mas fotos a las que quiere cambiar el tamaño. Luego se ingresa el tamaño en pixeles que quiere que la nueva foto tenga.
	MOSAIC: Se le presenta al usuario un listado de imagenes que tiene en editing area, el usuario debe seleccionar dos o mas fotos con las que ahra el mosaico (siendo la primera la base del mosaico). Luego el usuario ingresa el tamaño de la imagen que sera su mosaico y tambien el tamaño de las imagenes que lo compondra. ADVERTENCIA : Este metodo toma tiempo, puede que dure bastante timepo em procesar si es que el mosaico esta compuesto por mas de 1000 mini fotos.
	COLLAGE: Se le presenta al usuario un listado de imagenes que tiene en editing area, el usuario debe seleccionar una o mas fotos que quiere que esten en su collage. Luego el ususario ingresa el tamaño de su fondo, Este puede ser una foto seleccionada o puede ser una imagen de color solido que el usuario ingresa con formato RGB. Por ultimo elige el tamaño de las fotos que compondran su collage. 




----------------PASOS PARA MODIFICAR Y GUARDAR LA IMAGEN----------------

1) Crear la imagen nueva usando filtros o features.
2) Ver como se guardo tu nueva imagen. Si usaste un filtro entonces la misma imagen que seleccionaste es ahora tu imagen editada. Si usaste un collage o un mosaico , entonces la imagen base (la primera en ingresar) se le agregara el nombre "collage" o "mosaic".
3)Luego que identificaste la imagen editada, Seleccionala en "export images from de editingArea"
4)Vuelve al menu principal y elige "Export from My Library"
5)Sigues los pasos en pantalla y tu imagen se guardara exitosamente



----------------MANAGE LIBRARY----------------

Al seleccionar Manage Library, se le presenta al usuario otro menu con diversas opciones:

	SHOW MY LIBRARY : se le presenta al usuario un listado de las imagenes que se encuentra en su libreria.
	ADD LABEL : se le presenta al usuario una lista de  imagenes que se encuentra  en su libreria, debe seleccionar una e ingresar cual etiqueta quiere agregar.
	EDIT LABEL : se le presenta al usuario una lista de  imagenes que se encuentra  en su libreria, debe seleccionar una imagen y luego elegir que etiqueta quiere editar,
	por ultimo lo edita a su gusto. 
	DELETE LABEL : se le presenta al usuario una lista de  imagenes que se encuentra  en su libreria, debe seleccionar una e ingresar cual etiqueta quiere eliminar.
	SET CALIFICATION : se le presenta al usuario una lista de  imagenes que se encuentra  en su libreria, debe seleccionar una imagen y luego ingresar la calificacion que desea otorgarle
	a la imagen.
	DELETE IMAGE : se le presenta al usuario una lista de  imagenes que se encuentra  en su libreria, debe seleccionar una imagen para eliminar de su libreria.
	RESET LIBRARY : se le pide confirmacion al usuario y si es otorgada, entonces se borran todas las fotos de la libreria



----------------SEARCH MY LIBRARY----------------

Al seleccionar Search My Library, se le presenta al usuario otro menu con diversas opciones: 
	
	FACESEARCHER: Actualmente no esta implementado.


	SEARCHER: se le pide al usuario que ingrese una sentencia de busqueda.
		IMPORTANTE: para buscar los elementos deseados, primero se debe escribir el tipo y luego el nombre que se busca, se puede usar "and" y/o "or" para hacer una busqueda más compleja
		Ejemplo : Name:joaquin
			  Sentence:lamp or Name:carla
			  Surname:lopez and Sentence:faceplate
		Listado de todos los tipos de busqueda:
	 		-Sentence -> para los Simple Label
			-HairColor -> para los Person Label
			-EyesColor -> para los Person Label
			-Sex -> para los Person Label
			-Name -> para los Person Label
			-Surname -> para los Person Label
			-Birthdate -> para los Person Label
			-FaceLocation -> para los Person Label
			-Nationality -> para los Person Label
			-GeographicLocation -> para los Special Label
			-Address -> para los Special Label
			-Photographer -> para los Special Label
			-Photomotive -> para los Special Label
			-Selfie -> para los Special Label
			-ImageName -> para buscar en base por nombre de la foto
			-Calification -> para buscar en base por calificacion de la foto
			-Resolution -> para buscar en base por resolucion de la foto
			-AspectRatio -> para buscar en base a su aspectRatio de la foto
			-DarkClear -> para buscar en base si la foto es oscura o clara(bool true=clara)

	 INDICACION: cada vez que se escribe "or" en la busqueda, se inicia una sentencia nueva
	Ejemplo:  Sentence:lamp and Name:carla or Name:joaquin  --> ESO ESTA MAL
		Sentence:lamp or Name:carla or Sentence:lamp or Name:joaquin  --> ES LA BUSQUEDA DE MANERA CORRECTA



----------------MANAGE SMART LIST----------------

INDICACION: Las smart list son busquedas pre definidas, con el objetivo de escribir la sentencia 1 vez y en un futuro no tener que escribirla de nuevo

Al seleccionar Search My Library, se le presenta al usuario otro menu con diversas opciones: 

	SHOW MY SMART LIST: Muestra todas las smart list que existen por el momento, Muestran el pattern que seria la busqueda y las caracteristicas de la imagen que cumple con esa busqueda.
 	ADD SMART LIST: Se le pide al usuario ingresar una sentencia de busqueda, luego se le crea una smart list con los resultados.
	DELETE SMART LIST: Se le pide al usuario ingresar una sentencia de busqueda que desea eliminar.