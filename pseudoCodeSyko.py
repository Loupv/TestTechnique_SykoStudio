# temps passé 11h

# questions patrice : 
# - quel temps estimé ?
# - une préférence pour la syntaxe pseudo code ?
# - quels critères d'évaluation
# - faire une doc pour le système solaire ?
# - est-ce qu'on kick les participants, il n'est pas dit comment les participants doivent renseigner leur départ d'un stand

# les conventions accueillent un grand nombre de participants
# Une convention contient un certain nombre de stands
# chaque stand dispose d’un nombre limité de places disponibles, ainsi qu’une file d’attente. 

# Un participant occupe une place d’un stand pendant une durée moyenne déterminée par la nature de l’animation du stand.
# Chaque participant a établi au préalable son propre ordre de préférence pour les animations auxquelles il souhaite assister sur les différents stands.

# Ecrire en pseudo-code un processus qui répartit les participants en fonction de leurs préférences et de leur ordre d’arrivée. 
# Vous pourrez proposer des orientations au processus permettant d’optimiser cette répartition.

#################################

#TODO :
# - repartir en fonction des préférences et de l'ordre d'arrivée
# 	- les participants arrivées en premier sont affectés au premier stand libre qui a des places disponibles
#	- lorsqu'un participant est affecté à un stand, on retire une place du stand et on enlève ce stand de sa liste de préférences

# - lorsqu'un participant quitte un stand on doit le réaffecter à un autre stand



# on considère que les listes de participants et de stands sont déjà remplies
class Convention
	
	private Stand[] standArray # contient tous les objets stand, classés par numéro de stand allant de 0 à X 
	private list:Attendee attendeeList # liste des objets Attendee (les participants)
	private list:int arrivalList # contient les IDs des participants par ordre d'arrivée

	# ici on enregistre l'attendee sur le registre général lorsqu'il arrive à la convention
	public void AttendeeHasArrived(Attendee attendee)
		attendee.hasArrived = true
		AssignAttendeeToStand(attendee)

	# methode qui check si un stand est full
	public bool IsStandAvailable(int standID)
		if standArray[standID].availableSpotsOnStand > 0 return true 
		else return false
	

	# trie 
	public void SortAttendeeList()



	public AssignAttendeeToStand(Attendee attendee)
		# parcoure la liste des stands préférés de l'attendee et sélectionne le premier qui a des places disponibles
		# si un stand est trouvé alors on y réserve une place et on y assigne le participant

		bool vacantStandFound = False

		for(int i = 0, i< attendee.standIDPreferences.lenght, i++)

			if(IsStandAvailable(attendee.standIDPreferences[i])) # on récupère l'ID du stand et on check s'il a des places disponibles
				
				# si oui on assigne l'attendee au stand et on retire l'ID du stand de la liste des préférences
				print "Go to stand "+attendee.standIDPreferences[i]
				attendee.targettedStand = attendee.standIDPreferences[i]
				attendee.standIDPreferences.removeElement(i)
				vacantStandFound = True
			
			if(vacantStandFound) breakLoop

		if not(vacantStandFound && attendee.standIDPreferences > 0) # si par contre aucun stand libre n'a été trouvé, on prend le premier choix et on le met sur la liste d'attente
			standArray[attendee.standIDPreferences[0]].AddAttendeeToQueue(attendee)





class Stand

	private int standID
	private int meanTimePassedOnStand
	private int maxAttendee
    private int availableSpotsOnStand
	
	
	private list:int waitingQueue

	# faire une fonction qui permet à un participant de s'inscrire sur la liste si le nombre max de participant est atteint
	# autre fonction qui affecte les participants s'il reste des places

	public void AssignedAttendeeArrived(Attendee attendee)
		attendee.targettedStand = -1

	public void AddAttendeeToQueue(Attendee attendee)
		waitingQueue.Add(attendee)




class Attendee
	private int[] standIDPreferences # stocke la liste des IDs des stands dans l'ordre de préférence
	private int ID
	private int targettedStand = -1
	private bool isAssignedToAStand = false
	private bool hasArrived = false
