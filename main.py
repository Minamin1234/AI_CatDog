import sys
import dogcat

args = sys.argv
PICTURE = args[1]
AI_MODEL = "./cnn.h5"

MSG_BEGINPREDICT = "Begin Predict.."
MSG_SUCCESS = "Success"
MSG_FAILURE = "Failure"
MSG_ENDEDPREDICT = "Ended Predict."


if len(args) <= 1:
    print("Error: Not enough args.")
    exit()
    
if len(args) == 3:
    AI_MODEL = args[2]

print(MSG_BEGINPREDICT)
Result = dogcat.Predict(AI_MODEL,PICTURE)
res = ""
res += MSG_SUCCESS + "\n"

for i in Result[0]:
    res += str(i)
    res += "\n"
with open("result.txt","w") as f:
    f.write(res)
print(MSG_ENDEDPREDICT)
exit()