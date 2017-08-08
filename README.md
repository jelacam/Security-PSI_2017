# Security-PSI_2017
-- Istražiti ABAC model i uporediti ga sa RBAC modelom (Prednosti i mane oba modela)

-- Implementirati jednostavan servis koji podržava ABAC prema XACML standardu za specifikaciju autorizacionih politika: http://docs.oasis-open.org/xacml/3.0/xacml-3.0-core-spec-os-en.html#_Toc325047111.
Za ovaj projekat ograničenje na to da servis zna da tumači XACML autorizacione politike (vidi sekciju 3.3.), a za BSc širenje rada implementacijom kompletnog Data-flow model-a (vidi sekciju 3.1.).

BSc: Implementacija Data-flow modela tako da servis moze da donosi odluku o pristupu na osnovu dinamickih autorizacionih politika.
Akcenat na atributima kao sto su lokacija, vreme. 

Primer: Korisnik1 pripada jednoj korisnickoj grupi (uloga u RBAC modelu) i u odredjenom vremenskom periodu moze nesto da izvrsava. 
Korisnik2 pripada drugoj korisnickoj grupi i u nekom vremenskom periodu moze nesto drugo da izvrsava. 

Klijent rucno unosi lokaciju. Ukoliko bude vremena moze se prosiriti na automatsko detektovanje lokacije.




