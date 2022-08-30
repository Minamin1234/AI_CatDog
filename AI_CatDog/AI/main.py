import sys
import dogcat

args = sys.argv
PICTURE = args[1]
AI_MODEL = args[2]
RESULT_PATH = args[3]

MSG_BEGINPREDICT = "Begin Predict.."
MSG_SUCCESS = "Success"
MSG_FAILURE = "Failure"
MSG_ENDEDPREDICT = "Ended Predict."


if len(args) <= 3:
    print("Error: Not enough args.")
    exit()
    


print(MSG_BEGINPREDICT)
Result = dogcat.Predict(AI_MODEL,PICTURE)
res = ""
res += MSG_SUCCESS + "\n"

for i in Result[0]:
    print(i)
    res += str(i)
    res += "\n"
with open(RESULT_PATH,"w") as f:
    f.write(res)
print(MSG_ENDEDPREDICT)
exit()
#Args:[] [Picturepath] [ModelPath] [ResultPath]