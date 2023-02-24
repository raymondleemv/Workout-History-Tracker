# Workout-History-Tracker

This tracker allows you to track your workout history. There are 4 [controller](https://github.com/raymondleemv/Workout-History-Tracker/tree/main/WorkoutHistory/Controllers) files which handle requests to ExerciseType, Exercise, Workout, and WorkoutItem.

# Exercise Type
**GET** `http://localhost/ExerciseType/List`<br/>
This is the landing page of ExerciseType, you can view all the exercise types you have added here and carry out Create, Read, Update, Delete operations here.

# Exercise
**GET** `http://localhost/Exercise/List`<br/>
This is the landing page of Exercise, you can view all the exercises you have added here and carry out Create, Read, Update, Delete operations here.

# Workout
**GET** `http://localhost/Workout/List`<br/>
This is the landing page of Workout, you can view all the workouts you have added here and carry out Create, Read, Update, Delete operations here.

# Workout Item
**GET** `http://localhost/Workout/Details/{WorkoutID}`<br/>
The WorkoutItem table is a bridging table connecting the Workout table and the exercise table. You have to create a workout first, and then go to the details page of a workout to carry out Create, Read, Update, Delete operations on a workout item in that workout.