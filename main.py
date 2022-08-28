import sys
import dogcat

args = sys.argv
if len(args) <= 1:
    print("Error: Not enough args.")
    exit()

PICTURE = args[1]
AI_MODEL = "./cnn.h5"

if len(args) == 3:
    AI_MODEL = args[2]

MSG_BEGINPREDICT = "Begin Predict.."
MSG_SUCCESS = "Success"
MSG_FAILURE = "Failure"
MSG_ENDEDPREDICT = "Ended Predict."

print(MSG_BEGINPREDICT)
PIC = dogcat.Get_AI_Image(PICTURE)
Result = dogcat.Predict(AI_MODEL,PIC)
res = ""
res += MSG_SUCCESS
for i in Result[0]:
    res += str(i)
    res += "\n"
with open("result.txt","w") as f:
    f.write(res)
print(MSG_ENDEDPREDICT)
exit()