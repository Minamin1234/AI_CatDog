#みなみん猫の判定
import keras
import sys, os
import numpy as np
from PIL import Image
from keras.models import load_model
os.environ["CUDA_VISIBLE_DEVICES"] = "-1"

PIC = "./IMG_1134.JPG"
AI_MODEL = "./cnn.h5" #[dog,cat]
IMGSIZE = (64,64)

#何も処理を施していない生の画像を返します。
def Get_Image_Raw(path):
    img = Image.open(path)
    return img

#生の画像からAI用の画像に変換します。
def Convert(raw,imgsize=(64,64)):
    img = raw.convert("RGB")
    img = img.resize(imgsize)
    img = np.asarray(img)
    img = img / 255.0
    return img

#指定したパスからAI用の画像として取得します。
def Get_AI_Image(path,imgsize=(64,64)):
    img = Get_Image_Raw(path)
    img = Convert(img,imgsize)
    return img

#学習モデルから指定した画像を予測し、結果を返します。
def Predict(path_model,path_AIpic,imgsize=(64,64)):
    model = load_model(path_model)
    AIimg = Get_AI_Image(path_AIpic,imgsize)
    result = model.predict(np.array([AIimg]))
    return result
"""
Result = Predict(AI_MODEL,PIC,IMGSIZE)
Index = np.argmax(Result,axis=1)

print("----------判定----------")
if Index == 0:
    print("-> 犬🐕")
elif Index == 1:
    print("-> 猫🐈")
print("----------詳細----------")
Per_Dog = Result[0][0] * 100
Per_Cat = Result[0][1] * 100
print("犬🐕: " + f"{Per_Dog:.3f}" + "%")
print("猫🐈: " + f"{Per_Cat:.3f}" + "%")
"""
#print("----------画像----------")
#pic_width = 300
#img = Image.open(testpic)
#img.resize(( int(pic_width*(img.width/img.height)),pic_width ))