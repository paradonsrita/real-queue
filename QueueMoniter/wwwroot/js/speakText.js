function speakText(text, pauseDuration = 150) {
    const speechParts = text.split('.')
    let delay = 0

    speechParts.forEach((part, index) => {
        if (part.trim()) {
            delay += index === 0 ? 0 : pauseDuration;
            setTimeout(() => {
                const speech = new SpeechSynthesisUtterance(part.trim());
                speech.lang = 'th-TH';  // ตั้งค่าเป็นภาษาไทย
                speech.rate = 0.7;
                speech.pitch = 1;

                const voices = window.speechSynthesis.getVoices();
                speech.voice = voices.find(voice => voice.lang === 'th-TH');


                window.speechSynthesis.speak(speech);

            }, delay);
        }
    })

    
}