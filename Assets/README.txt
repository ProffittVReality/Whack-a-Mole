Whack-a-Mole README

Trial Manager
	
	Calibration Settings

		Pass Num: the number of balloons popped needed 
			to "pass" calibration
		Initial Lifetime: starting balloon lifetime
		Pass Round Decrement: amount balloon lifetime 
			will decrease after a passed calibration 
			round
		Fail Round Increment: amount balloon lifetime 
			will increase after a failed calibration 
			round

	Num Trials: The number of data trials (after calibration)
		NOTE: may want to increase this number for 
			combined drift rounds

	Next Trial: Set the key to press to start each new trial

ResearchGUIScript

	Calibration File Name: set name of calibration data file
	Data File Name: set name of file to record trial data

	NOTE: do NOT need to include ".csv", this addition is built 
		into the script

DriftController

	Drift Settings

		Controller Rotation Number: number of different 
			controller rotation amounts
		Controller Rotation Interval: interval between 
			each controller rotation amount
		
		Controller Trans Number: number of different 
			controller translation amounts
		Controller Trans Interval: interval between 
			each controller translation amount

		Room Rotation Number: number of different 
			headset rotation amounts
		Room Rotation Interval: interval between 
			each headset rotation amount

		NOTE: for all rotation / translation numbers, 
			there will be about twice as many different 
			rotation amounts used in the trials 
			(positive and negative of each amount)

		Example: given controller rotation number 5 
			and controller rotation interval 5, 
			the rotation values used will be 
			(-20, -15, -10, -5, 0, 5, 10, 15, 20)

		NOTE: rotation amounts are in degrees, 
			translation amounts are in Unity units

Balloon Controller

	Low Interval Limit: lower bound for randomly 
		generated interval between balloon spawns
		NOTE: values below 0.6 are generally too quick, 
			causing errors in the accuracy of 
			pop-attempt detection

	High Interval Limit: upper bound for randomly 
		generated interval between balloon spawns

