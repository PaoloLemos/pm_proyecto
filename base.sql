Create Database VozDelEste
go
use VozDelEste
go
CREATE TABLE Clientes (
    CI VARCHAR(20) NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL,
	fotoPerfil NVARCHAR(250)
);
go



CREATE TABLE Clima (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL,
    Temperatura DECIMAL(5,2),
    Descripcion NVARCHAR(100)
);
go


CREATE TABLE Programas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Imagen NVARCHAR(255),
    Descripcion NVARCHAR(MAX)
);
go

Create TABLE Conductores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProgramaId INT NULL ,
    Nombre NVARCHAR(100) NOT NULL,
    Bio NVARCHAR(MAX),
	foto nvarchar(MAX),
    FOREIGN KEY (ProgramaId) REFERENCES Programas(Id)
);
go

CREATE TABLE Comentarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteCI VARCHAR(20),
    ProgramaId INT,
    Comentario NVARCHAR(MAX),
    Fecha DATETIME NOT NULL,
    FOREIGN KEY (ClienteCI) REFERENCES Clientes(CI),
    FOREIGN KEY (ProgramaId) REFERENCES Programas(Id)
);
go
CREATE TABLE Cotizaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL,
    TipoMoneda NVARCHAR(3),
    Valor DECIMAL(18,4) NOT NULL,
    CHECK (TipoMoneda IN ('USD', 'EUR', 'BRL'))
);

go

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Contrasena NVARCHAR(255) NOT NULL,
	RolID int NOT NULL
	Foreign key(RolId)REFERENCES Roles(RolId) 
);

go

create table Roles
(
RolId int identity(1,1) primary key,
Nombre nvarchar(50) not null unique 
)

go
create table Permisos (
PermisoId INT IDENTITY(1,1) PRIMARY KEY,
Nombre nvarchar(100) not null unique 
)

go

create table RolesPermisos
(
RolId int NOT NULL,
PermisoId int NOT NULL,
rolPermisoId int Identity(1,1) primary key
foreign key (RolId) REFERENCES Roles(RolId),
foreign key (PermisoId) REFERENCES Permisos(PermisoId)

)
go

CREATE TABLE Noticias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Contenido NVARCHAR(MAX),
    FechaPublicacion DATE NOT NULL,
    Imagen NVARCHAR(255)
);
go
CREATE TABLE Patrocinadores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    [Plan] INT NOT NULL,
	Imagen Nvarchar(250)   
);

go

CREATE TABLE ProgramacionHoraria (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProgramaId INT NOT NULL,
    DiaSemana NVARCHAR(20) NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    FOREIGN KEY (ProgramaId) REFERENCES Programas(Id)
);

go
--necesitamos ejecutar esto antes si no lo tenemos
Insert Into Roles(Nombre)
values
('Administrador'),
('Editor'),
('Cliente')

go


-- tabla necesaria para que el programa tengo ususarios , el mas importante el admin, la contraseña es 0000
INSERT INTO Usuarios (Nombre, Email, Contrasena, RolID)
VALUES 
  ('Cliente Uno',    'cliente1@ejemplo.com', 'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 3),
  ('Cliente Dos',    'cliente2@ejemplo.com', 'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 3),
  ('Cliente Tres',   'cliente3@ejemplo.com', 'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 3),
  ('Cliente Cuatro', 'cliente4@ejemplo.com', 'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 3),
  ('Editor Maestro', 'editor@ejemplo.com',    'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 2),
  ('Paolo',  'admin@gmail.com',  'AM4AZA985GaAJOFZcRIEW7qcV9qcutQQzbh4ragOSEZ1gwdDvVWnFfeXBC+x7AsGgA==', 1);

  go




  --creamos permisos , puede que aun falten
INSERT INTO Permisos (Nombre) VALUES
  ('Modificar Clientes'),
  ('Ver Clientes'),
  ('Modificar Usuarios'),
  ('Ver Usuarios'),
  ('Modificar Noticias'),
  ('Ver Noticias'),
  ('Modificar Patrocinadores'),
  ('Ver Patrocinadores'),
  ('Modificar Programas'),
  ('Ver Programas');


  go

  --Le damos todos los permisos al admin
  INSERT INTO RolesPermisos (RolID, PermisoID)
SELECT
  1 AS RolID,
  PermisoID
FROM Permisos;

go






-- le damos permisos al editor, si no los tiene ejecutamos lo siguiente
 INSERT INTO RolesPermisos (RolID, PermisoID)
values 
(2,5),
(2,6),
(2,9),
(2,10)

go
INSERT INTO Cotizaciones (Fecha, TipoMoneda, Valor) VALUES
('2025-07-10 08:00:00', 'EUR', 0.9201),
('2025-07-09 09:00:00', 'EUR', 0.9215),
('2025-07-08 10:00:00', 'EUR', 0.9193),
('2025-07-07 11:00:00', 'EUR', 0.9227)

go
INSERT INTO Clima (Fecha, Temperatura, Descripcion)
VALUES 
(CAST(GETDATE() AS DATE), 22.5, 'cielo claro'),
(CAST(DATEADD(DAY, -1, GETDATE()) AS DATE), 19.3, 'nubes dispersas'),
(CAST(DATEADD(DAY, -2, GETDATE()) AS DATE), 17.8, 'lluvia ligera'),
(CAST(DATEADD(DAY, -3, GETDATE()) AS DATE), 16.2, 'nublado'),
(CAST(DATEADD(DAY, -4, GETDATE()) AS DATE), 21.0, 'cielo claro'),
(CAST(DATEADD(DAY, -5, GETDATE()) AS DATE), 23.4, 'tormenta');

go

INSERT INTO Noticias (Titulo, Contenido, FechaPublicacion, Imagen) VALUES
('Aumentan los Ataques de Perros Peligrosos en Artigas', 'Maldonado, 11 de julio de 2025 — En las últimas semanas, varios ataques protagonizados por perros considerados potencialmente peligrosos han generado alarma en diferentes barrios de Montevideo y Canelones, encendiendo el debate sobre la tenencia responsable, la regulación de razas específicas y la falta de control en espacios públicos.  🔺 Tres ataques en menos de 10 días El pasado jueves, un niño de 7 años fue hospitalizado en estado grave tras ser atacado por un perro de raza American Bully en el barrio La Teja. El animal, que se encontraba suelto sin bozal ni correa, pertenecía a un vecino que ya había recibido advertencias por incidentes anteriores.  Este caso se suma a otros dos episodios registrados en menos de 10 días: uno en Pando, donde una mujer resultó con lesiones severas en los brazos tras intentar proteger a su mascota de un Rottweiler, y otro en la zona de Malvín Norte, donde un Dogo Argentino escapó de un predio y mordió a un repartidor.  📊 Cifras en ascenso preocupan a las autoridades Según datos brindados por la Dirección de Bienestar Animal y Zoonosis, en el primer semestre de 2025 se reportaron 312 incidentes con mordeduras caninas, de los cuales al menos 89 involucraron razas categorizadas como potencialmente peligrosas, como el Pitbull Terrier, Rottweiler, Dogo Argentino, Fila Brasileño y sus cruces.  “Estamos observando un aumento del 28% respecto al mismo período del año pasado”, explicó la Dra. Laura Ferreyra, directora de la unidad. “No se trata solo de razas, sino de condiciones de tenencia, falta de socialización y negligencia por parte de los dueños”, agregó.  ⚖️ ¿Qué dice la ley uruguaya? En Uruguay, la Ley N.º 18.471 establece normas para la tenencia responsable de animales, pero no clasifica oficialmente a ninguna raza como "peligrosa". A diferencia de países como España o Reino Unido, donde existen listas oficiales con requisitos específicos (como licencias, seguros obligatorios y adiestramiento), en Uruguay la regulación es más general.  No obstante, algunos municipios han comenzado a emitir normativas complementarias. En Canelones, por ejemplo, se exige bozal para ciertas razas en espacios públicos, y Montevideo evalúa aplicar medidas similares.  El diputado Eduardo Blanco, integrante de la Comisión de Medio Ambiente y Bienestar Animal, declaró que “urge modernizar la normativa y poner el foco en la prevención”, y anunció que presentará un proyecto de ley para crear un Registro Nacional de Propietarios de Perros Potencialmente Peligrosos, además de establecer sanciones más severas por omisión de cuidado.  👥 La otra cara: criadores y defensores de estas razas responden Diversos grupos de tenencia responsable y criadores registrados sostienen que el problema no radica en la raza, sino en la educación. “Los perros no nacen agresivos. Muchos de estos animales terminan en manos equivocadas, usados para peleas, criados sin control ni afecto”, explicó Mariana Gadea, fundadora de la Asociación Canina del Uruguay.  También señalan que las medidas restrictivas pueden fomentar la estigmatización. “Se está generando miedo en vez de fomentar la responsabilidad. No queremos ver razas prohibidas, sino propietarios capacitados”, opinó Gadea.  🐾 ¿Y ahora qué? Mientras el debate se intensifica, el Ministerio del Interior informó que colaborará con las intendencias para reforzar controles en espacios públicos y aplicar multas más severas a dueños que no cumplan con normas mínimas como correa, bozal y vacunación al día.  Por su parte, organizaciones animalistas insisten en la necesidad de campañas de educación ciudadana y castración obligatoria. “Hay una crisis de abandono y reproducción irresponsable que no se va a resolver solo prohibiendo razas”, afirmó Sofía Menéndez, voluntaria de Refugio Patitas Libres.  Mientras tanto, vecinos de los barrios más afectados reclaman acciones urgentes: “No puede ser que nuestros hijos no puedan ir a la plaza tranquilos”, lamentó Lorena Duarte, madre de la víctima de La Teja.', '2025-07-11', '/Content/imagenes/Noticias/perro.jpg'),
('Sequia Persistente en el Interior del País Pone en Alerta a Productores', 'Tacuarembó, 11 de julio de 2025 — La prolongada sequía que afecta al norte y centro del país desde fines de 2024 ya está teniendo consecuencias críticas en el sector agropecuario. Productores denuncian pérdidas millonarias, falta de acceso al agua y deterioro del suelo, mientras el gobierno analiza nuevas líneas de asistencia para el campo.  ☀️ Tres meses sin lluvias significativas Según el Instituto Uruguayo de Meteorología (Inumet), en departamentos como Rivera, Tacuarembó, Durazno y Cerro Largo no se registran lluvias importantes desde hace más de 90 días. Las temperaturas elevadas y los vientos constantes han agravado el fenómeno, disminuyendo drásticamente los niveles de humedad en el suelo.  “La situación es insostenible. Los tajamares están secos, el pasto no crece, y el ganado está flaco y enfermando”, relató Jorge Barrios, productor ganadero de Paso de los Toros. “Nunca vivimos algo así, y no sabemos cuánto más va a durar”.  🐄 Ganadería y agricultura, los más golpeados La Asociación Rural del Uruguay estimó pérdidas de más de 300 millones de dólares en lo que va del año, principalmente por muerte de animales, caída en la producción de leche y carne, y pérdida de cultivos como soja y maíz.  En zonas agrícolas, muchos pequeños productores han tenido que abandonar los cultivos de secano o vender maquinaria para solventar deudas. “Sembramos con esperanza y estamos cosechando pérdida”, comentó María del Carmen Cabrera, agricultora de Sarandí del Yí.  🌐 Emergencia agropecuaria y ayudas estatales El Ministerio de Ganadería, Agricultura y Pesca declaró la emergencia agropecuaria en 11 departamentos y activó líneas de crédito blando y entrega de raciones. Sin embargo, muchos productores aseguran que los apoyos no llegan a tiempo o son insuficientes.  “El agua es ahora más importante que cualquier subsidio económico. Necesitamos obras hídricas y soluciones estructurales, no solo parches”, afirmó Luis Frugoni, presidente de la Federación Rural.  El gobierno evalúa utilizar fondos del FONDES para ampliar pozos semisurgentes y acelerar proyectos de captación de agua de lluvia, además de coordinar con OSE el abastecimiento con camiones cisterna.  🌱 Cambio climático y planificación a largo plazo Expertos del Instituto de Clima y Agua del INIA advierten que estos eventos extremos serán cada vez más frecuentes. “No podemos pensar en la sequía como una excepción. Hay que rediseñar nuestra matriz productiva para adaptarnos a un nuevo régimen climático”, explicó la Dra. Cecilia Bianchi.  Algunas organizaciones rurales ya están impulsando sistemas de rotación de cultivos, pasturas resistentes a la sequía y reservorios de agua inteligentes, pero el cambio es lento y requiere apoyo técnico y financiero.  🤝 Solidaridad entre productores Pese a la crisis, se han multiplicado las redes de ayuda mutua. Varios grupos de WhatsApp y cooperativas organizan envíos de forraje desde zonas menos afectadas hacia campos en emergencia. “Esto nos hace sentir menos solos. El campo se cuida entre todos”, expresó emocionado Federico Ottonello, un productor de Young.', '2025-07-11', '/Content/imagenes/Noticias/sequia.jpg'),
('Estudiantes con Ansiedad debido a alta exigencia en los Liceos', 'Maldonado, 11 de julio de 2025 — La ansiedad, el agotamiento emocional y los cuadros de estrés crónico entre estudiantes de educación secundaria uruguaya están alcanzando niveles preocupantes. Docentes, padres y psicólogos alertan sobre el impacto que tiene la presión académica en el bienestar mental de los adolescentes, y exigen cambios profundos en el enfoque educativo actual.  🧠 “Siento que si no saco buenas notas, fracaso en la vida” La frase de Valentina, una estudiante de 14 años de un liceo público de Montevideo, refleja el sentimiento de muchos adolescentes uruguayos. “Duermo mal, me levanto con dolor de panza, tengo que rendir tres pruebas por semana y si me va mal siento que decepciono a mis padres”, cuenta.  Casos como el suyo no son aislados. Según un informe reciente de la Sociedad Uruguaya de Pediatría, el 62% de los adolescentes entre 12 y 17 años experimenta niveles de ansiedad por rendimiento académico, y un 35% reconoce haber tenido pensamientos de abandono escolar o autocrítica extrema por no “dar la talla”.  🏫 El liceo como fuente de presión más que de aprendizaje Para muchos estudiantes, el liceo ha dejado de ser un espacio de exploración y crecimiento para convertirse en una fábrica de estrés. Profesores presionados por planes curriculares extensos, exámenes acumulativos y una cultura de calificación constante terminan trasladando sin querer esa presión al aula.  “Nos piden que evaluemos con objetividad, pero no nos dan tiempo para enseñar con profundidad”, explica Mariana Sosa, profesora de Biología. “Los adolescentes sienten que deben ser perfectos todo el tiempo, y cuando no lo logran, se hunden”.  En zonas más vulnerables, la presión se combina con condiciones adversas: falta de conectividad, problemas familiares y carga laboral fuera del estudio. “A veces los chiquilines no tienen ni para imprimir un trabajo y sin embargo se les exige igual que a los demás”, señala un docente del liceo N.º 41 de Canelones.  🧑‍⚕️ Psicólogos en alerta: trastornos en aumento Desde la Facultad de Psicología de la Udelar afirman que aumentaron las consultas por trastornos de ansiedad, fobia escolar, insomnio infantil, autoexigencia extrema y trastornos alimentarios ligados a estrés académico.  “El sistema está formando chicos que creen que el valor personal se mide en números”, sostiene la psicóloga escolar Natalia Romero. “Cada vez más estudiantes llegan a consulta con síntomas de ‘burnout’ que antes solo veíamos en adultos”.  El problema se agrava por la escasez de orientación psicológica en los centros educativos. En muchos liceos hay un solo psicólogo para cientos de estudiantes, o directamente ninguno.  🔄 ¿Qué se puede hacer? Expertos proponen una transformación integral del sistema educativo: menos carga memorística, más acompañamiento emocional, evaluaciones más humanas y espacios reales para que los estudiantes expresen sus miedos, intereses y necesidades.  Además, se reclaman más instancias de diálogo entre docentes, padres y estudiantes, así como la implementación de programas de salud mental escolar permanentes y con presupuesto asignado.  Desde el Codicen se anunció que se está elaborando un nuevo marco para la “Educación Socioemocional en Secundaria”, que podría comenzar a aplicarse en fase piloto a partir de 2026.  👨‍👩‍👧 Familias agotadas, chicos exigidos Mariela, madre de un estudiante de segundo año, resume el sentir de muchos padres: “Yo no quiero que mi hijo saque 12 en todo. Quiero que esté feliz, que se sienta capaz. Pero el sistema lo empuja a exigirse como si fuera un robot. No quiero que se quiebre por una nota”.', '2025-07-11', '/Content/imagenes/Noticias/estudiantes.jpg'),
('Dembélé Deslumbra en la Final del Mundial de Clubes', 'Yeda, Arabia Saudita – 11 de julio de 2025 — Ousmane Dembélé ha alcanzado el punto más alto de su carrera. Con una actuación inolvidable en la final del Mundial de Clubes, donde anotó dos goles y brindó una asistencia clave en la victoria del Paris Saint-Germain sobre el Real Madrid por 3-1, el extremo francés no solo llevó a su equipo a conquistar el título más prestigioso del fútbol de clubes, sino que también se posicionó como el gran favorito para alzar el Balón de Oro 2025.  ⚽ Una noche mágica ante el eterno rival En un estadio repleto en Yeda, el duelo entre PSG y Real Madrid fue una verdadera final anticipada. Pero el protagonista fue uno solo: Dembélé, que desplegó todo su repertorio con regates eléctricos, desmarques letales y una definición quirúrgica.  Abrió el marcador a los 12 minutos con un disparo cruzado imparable tras una jugada individual desde la banda derecha. Luego, asistió a Mbappé con un pase entre líneas digno de videoconsola, y sentenció el encuentro con un zurdazo desde fuera del área que dejó sin reacción a Lunin.  Su actuación fue ovacionada incluso por parte de la hinchada rival. “Hoy Ousmane jugó a otro nivel. Fue un recital”, declaró Ancelotti en conferencia de prensa, con gesto resignado.  🏆 Temporada consagratoria La final solo fue el broche de oro de una temporada brutal para Dembélé, quien ha mantenido un nivel de regularidad y explosión pocas veces visto en su carrera. Tras años marcados por lesiones y altibajos, el exjugador del Barcelona parece haber alcanzado su madurez futbolística con 28 años.  23 goles y 18 asistencias en la Ligue 1  Campeón de Ligue 1, Copa de Francia, Champions League y Mundial de Clubes  MVP en tres finales  Protagonista absoluto en partidos clave  “Está sano, enfocado, y juega con alegría. Es el mejor jugador del mundo hoy por hoy”, sentenció Luis Enrique tras la consagración.  🥇 France Football lo coloca al frente por el Balón de Oro La prestigiosa revista France Football, encargada de entregar el Balón de Oro, ya lo incluye como número uno en su ranking provisional 2025, superando a figuras como Jude Bellingham, Kylian Mbappé y Erling Haaland.  En su edición más reciente, la revista titula: “El arte de renacer: Dembélé y su año de oro”, destacando no solo sus números, sino su influencia en los partidos decisivos.  “El premio no siempre es para el más regular, sino para quien brilla más intensamente en los momentos grandes. Este fue el año de Ousmane”, asegura el periodista Thierry Marchand, del comité de votación.  🌍 Una reivindicación personal Dembélé ha vivido una carrera marcada por expectativas gigantescas desde su fichaje millonario por el Barcelona en 2017. Pero recién ahora, tras años de crítica y dudas, parece haber encontrado su identidad como líder futbolístico.  “Siempre creí en mí. Me dijeron muchas veces que estaba acabado. Pero hoy siento que es solo el principio”, declaró emocionado luego del partido, con el trofeo en una mano y la medalla de campeón colgando del cuello.', '2025-07-11', '/Content/imagenes/Noticias/dembo.jpg');

go

INSERT INTO Programas (Nombre, Imagen, Descripcion) VALUES
('Mujeres al Frente', '/Content/imagenes/Programas/podcastmujeres.jpg', 'En el primer episodio de "Mujeres al Frente", tres voces femeninas fuertes, inteligentes y con mucha personalidad se reúnen para analizar el mundo del espectáculo, los medios y la actualidad con una mirada crítica, entretenida y muy bien informada.    ✨ Valentina, periodista con años de experiencia en televisión.  🧠 Carla, especialista en comunicación y redes sociales.  🎤 Sofía, columnista cultural y amante del análisis con humor.    En este episodio:    📺 Lo que no se vio detrás de cámaras en el último reality éxito.    💬 La polémica por los dichos de una actriz consagrada.    📸 Influencers, contratos publicitarios y lo que se esconde detrás del feed perfecto.    Con secciones como "El Dato Fuerte", "¿Qué hay de cierto?" y "Lo que se viene", este programa promete información, entretenimiento y un toque de ironía… siempre con respeto y estilo.    '),
('412', '/Content/imagenes/Programas/412.jpg', 'En este nuevo episodio, Davo Xeneize junto a su inseparable compañero Brunito (o el invitado sorpresa del día), se sumergen en la actualidad caliente del mundo Boca. Análisis sin filtro, opinión picante y todo el folklore bostero que los caracteriza. Desde rumores de refuerzos hasta internas dirigenciales, pasando por el análisis del último partido, no queda tema sin tocar.    Con la chispa de siempre, humor, debate y ese estilo barrial que los hizo virales, el programa #412 es un clásico más de este canal que ya es religión para el hincha Xeneize.    🔥 Momentos destacados:    ¿Tiene que seguir el DT?    ¿Qué pasa con los referentes del plantel?    Las declaraciones que encendieron la polémica.    Como siempre, con mate en mano, camisetas puestas y el corazón azul y oro, Davo y compañía te traen otra dosis de pasión bostera.'),
('Malos pensamientos', '/Content/imagenes/Programas/petinatti.jpg', '"Malos Pensamientos" es mucho más que un programa de radio: es un ritual diario para miles de uruguayos. Con su estilo ácido, provocador e inconfundible, Petinatti combina humor, actualidad, participación del público y un toque de incorrección que hace honor al nombre del ciclo.    📞 Segmentos como "La Mano en el Corazón" (confesiones reales y muchas veces insólitas),  🎭 Personajes únicos como El Gaucho Rolando,  🎤 Entrevistas con figuras nacionales e internacionales,  📢 Opiniones filosas sobre política, sociedad y cultura pop...    Todo forma parte de un programa que lleva décadas marcando agenda y generando conversación, siempre con la ironía punzante y el carisma provocador de su conductor.    "Malos Pensamientos" es el espacio donde lo políticamente correcto queda afuera... y la risa entra sin pedir permiso.'),
('Uruguay Music', '/Content/imagenes/Programas/music.jpg', 'Uruguay Music es el programa donde la música uruguaya tiene voz, historia y futuro. Desde clásicos del candombe, el rock y la murga, hasta lo más nuevo del indie, el trap y el pop local, Camila y Rodrigo te acompañan en un viaje sonoro por todo lo que está pasando en el país y más allá.    🎶 Entrevistas con artistas emergentes y consagrados  📀 Lanzamientos, discos recomendados y backstage exclusivos  📊 Rankings semanales con votación del público  📍 Coberturas de festivales, ciclos y movidas culturales    Con una producción cuidada, opiniones sin cassette y pasión por lo nuestro, Uruguay Music es el punto de encuentro entre los músicos, el público y toda la riqueza sonora que el país tiene para ofrecer.    Escuchalo, compartilo, vivilo. Porque Uruguay también suena.');

go

INSERT INTO ProgramacionHoraria (ProgramaId, DiaSemana, HoraInicio, HoraFin) VALUES
(1, 'Martes', '19:19:00', '23:02:00'),
(2, 'Viernes', '21:02:00', '23:00:00'),
(3, 'Jueves', '19:19:00', '23:02:00'),
(4, 'Sábado', '21:02:00', '23:00:00'),
(5, 'Lunes', '10:00:00', '12:00:00'),
(6, 'Miércoles', '12:00:00', '16:00:00');

go


--para ver si anduvo lo anterior
SELECT RP.rolPermisoId, R.Nombre AS Rol, P.Nombre AS Permiso
FROM RolesPermisos RP
JOIN Roles    R ON RP.RolID     = R.RolID
JOIN Permisos P ON RP.PermisoID = P.PermisoID
WHERE RP.RolID = 1;


select * from Roles

select * from Permisos
SELECT * FROM ProgramacionHoraria
select * from Noticias
select * from Programas
