# Handleiding En Concepten

> **Note:** De GUI en source code is volledig in het Engels.

## Handleiding

1. Bij het opstarten:
	1. Je kan inloggen als 5 verschillende rollen:
		- Een admin
		- Een student
		- Een leerkracht
		- Een bedrijfsmedewerker
		- Een bedrijfsmanager
	2.  Je kan een bedrijf registreren.
		- Voor het bedrijf wordt een naam verwacht.
		- Voor de bedrijfsmanager wordt een email, wachtwoord, voornaam en achternaam verwacht.
2. In het dashboard:
	- Bedrijfsmanagers kunnen hier 2 extra dingen doen:
		1. Stagevoorstellen toevoegen, hiervoor wordt een titel en beschrijving verwacht.
		2. Bedrijfsmedewerkers registreren, hiervoor wordt een email, wachtwoord, voornaam en achternaam verwacht.
	- Studenten kunnen hier 1 extra ding doen:
		1. Een CV uploaden, dit worth gedaan via een file explorer pop-up dat filtert op PDFs.
	- Iedereen kan vanaf hier naar de stagelijst gaan.
3. In de stagelijst, hier ka n iedereen kan stages bekijken:
	- Admins kunnen alle nog niet en wel goedgekeurde (ook toegekende) stages zien, hier kunnen ze 2 dingen doen:
		1. Stages goedkeuren (afkeuren is niet geimplementeerd).
		2. Goedgekeurde stages toekennen aan een mentor, student en leerkracht, dit gebeurt allemaal via emails.
	- Studenten kunnen alle goedgekeurde en nog niet toegekende stages zien samen met stages toegekend aan zichzelf, hier kunnen ze 1 ding doen.
		1. Zich kandidaat stellen voor goedgekeurde stages Na een CV te uploaden.
	- Leerkrachten kunnen hier alle nog niet en wel goedgekeurde (ook toegekende) stages zien, maar geen speciale acties uitvoeren.
	- Bedrijfsleiders kunnen alle nog niet en wel goedgekeurde (ook toegekende) stages van hun bedrijf zien, hier kunnen ze 1 ding doen:
		1. Het toekenen van goedgekeurde stages aan een kandidaat.
	- Bedrijfsmedewerkers kunnen hier alle nog niet en wel goedgekeurde (ook toegekende) stages van hun bedrijf zien, maar geen speciale acties uitvoeren.
> Het stage detail venster wordt geactiveerd door op een stage te dubbelklikken.
4. In het stage detail venster kan je meer info vinden over een specifieke stage, hier kan je 3 soorten stages onderscheiden.
		1.  Nog niet goedgekeurde stages: hier kan je niets mee doen tot een admin hem goedkeurt.
		2. Goedgekeurde stages maar nog niet toegekend:
			- Admins, leerkrachten en bedrijfsmedewerkers van het bedrijf van de stage kunnen hier kandidaten zien en hun CV downloaden.
			- Studenten kunnen zien welk bedrijf de stage van is.
		3. Goedgekeurde stages toegekend aan een student:
			- Admins, bedrijfsmedewerkers, leerkrachten en de student kunnen de mentor, student en leerkracht verbonden aan de stage zien. Ook kunnen ze de evaluatie bekijken.
				- Admins kunnen de volledige evaluatie aanpassen.
				- De mentor kan zijn evaluaties invullen en de notitie aanpassen.
				- De leerkracht kan zijn evaluatie invullen en de notitie ook aanpassen.
				- De volledige score wordt automatisch berekend.

## Concepten

- Algemene .NET concepten
	- In alle projecten.
- Namespaces
	- In alle projecten.
- Stijlafspraken
	- In alle projecten.
- Nullable reference types
	- In alle projecten. Veel in de `Data` en `Logic` class-libraries.
- Properties
	- Voornamelijk in de `GLobals` class-library.
- Indexers
	- In de `Data` class-library, lijn 290.
- Iterators
	- In alle projecten. Voornamelijk in de `Data` class-library, in de methoden die een `List<Internship>` collectie terug brengen.
- Delegates
	- In de `Data` class-library, lijn 123.
- Lambda expressions
	- In het `PresentationGUI` WPF project, in de `public partial class DashBoardWindow : Window` op lijn 64.
- Events
	-  In het `PresentationGUI` WPF project, in de `public partial class DashBoardWindow : Window` op lijn 64.
- LINQ
	- In de `Data` class-library:
		- Methoden:
			1. Op lijn 96.
			2. Op lijn 192.
			3. Op lijn 256.
			4. Op lijn 383.
		- Queries:
			1. Op lijn 251.
			2. Op lijn 269.
			3. Op lijn 318.
			4. Op lijn 359.
- Interpolated strings
	- In de `Logica` class-library, op lijn 147.
- Serialization
	- In de `Data` class-library, op lijn 133.
- Structs
	- In de `Globals` class-library, op lijn 21.
- Interfaces
	- In de `LogicaInterface` en `DataInterface` class-libraries.
- Meerlagenmodel
	-  In alle projecten.
- Generic collections
	- In de `Data` class-library, op lijn 242.
- GUI met XAML
	- In het `PresentationGUI` WPF project.