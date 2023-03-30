import wave
import matplotlib.pyplot as plt

import numpy as np

import os

f = wave.open("audio.wav",'rb")
params = f.getparams(nframes)
nchannels,sampwidth,framerate,nframes = params[:4]
print(framerate)


strData= f.readframes(nframes)
waveData = np.frombuffer(strData,dtype=np.int16)
waveData = waveData*1.0/(max(abs(waveData)))

time = np.arange(0,nframes)*(1.0/framerate)

plt.plot(time,waveData)
plt.xlabel("time(s")
plt.ylabel("Amplitude")
plt.title("Single channel wavedata")
plt.show()
