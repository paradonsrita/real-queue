// ฟังก์ชันนี้ใช้สำหรับพูดข้อความในภาษาไทย โดยแบ่งข้อความเป็นส่วน ๆ และหน่วงเวลาแต่ละส่วน
function speakText(text, pauseDuration = 0) {
    const speechParts = text.split('.');  // แบ่งข้อความตามจุด (.)
    let delay = 0;

    speechParts.forEach((part, index) => {
        if (part.trim()) {
            delay += index === 0 ? 0 : pauseDuration;  // หน่วงเวลา
            setTimeout(() => {
                const speech = new SpeechSynthesisUtterance(part.trim());
                speech.lang = 'th-TH';  // ตั้งค่าเป็นภาษาไทย
                speech.rate = 0.9;  // ความเร็วในการพูด
                speech.pitch = 1;  // ความสูงของเสียง

                const voices = window.speechSynthesis.getVoices();
                speech.voice = voices.find(voice => voice.lang === 'th-TH');  // เลือกเสียงภาษาไทย

                window.speechSynthesis.speak(speech);  // เริ่มพูด
            }, delay);
        }
    })
}
