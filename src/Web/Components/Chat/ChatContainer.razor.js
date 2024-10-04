var SpeechSDK;
var recognizer;

export function initializeSpeechRecognition() {
    if (!!window.SpeechSDK) {
        SpeechSDK = window.SpeechSDK;
        console.log("SpeechSDK loaded");
    }
}

export function startContinuousRecognitionAsync(dotNetHelper, language) {
    if (typeof recognizer === "undefined") {
        var speechConfig = SpeechSDK.SpeechConfig.fromSubscription("e3bb29e86fcd4aebaf96684a6893bcea", "swedencentral");
        speechConfig.speechRecognitionLanguage = language;
        var audioConfig = SpeechSDK.AudioConfig.fromDefaultMicrophoneInput();
        recognizer = new SpeechSDK.SpeechRecognizer(speechConfig, audioConfig);
    }

    recognizer.recognizing = function (s, e) {
        console.log(`RECOGNIZING: Text=${e.result.text}`);
    };

    recognizer.recognized = function (s, e) {
        if (e.result.reason === SpeechSDK.ResultReason.RecognizedSpeech) {
            console.log(`RECOGNIZED: Text=${e.result.text}`);
            dotNetHelper.invokeMethodAsync("AddUserMessageFromSpeech", e.result.text);
        } else if (e.result.reason === SpeechSDK.ResultReason.NoMatch) {
            console.log("NOMATCH: Speech could not be recognized.");
        }
    };

    recognizer.canceled = function (s, e) {
        console.log(`CANCELED: Reason=${e.reason}`);

        if (e.reason === SpeechSDK.CancellationReason.Error) {
            console.log(`CANCELED: ErrorCode=${e.errorCode}`);
            console.log(`CANCELED: ErrorDetails=${e.errorDetails}`);
        }

        recognizer.stopContinuousRecognitionAsync();
    };

    recognizer.sessionStarted = function (s, e) {
        console.log(`SESSIONSTARTED: SessionId=${e.sessionId}`);
    };

    recognizer.sessionStopped = function (s, e) {
        console.log(`SESSIONSTOPPED: SessionId=${e.sessionId}`);
        recognizer.stopContinuousRecognitionAsync();
    };

    recognizer.startContinuousRecognitionAsync();
}

export function stopContinuousRecognitionAsync(dotNetHelper) {
    if (recognizer) {
        recognizer.stopContinuousRecognitionAsync(
            function () {
                console.log("Recognition stopped.");
            },
            function (err) {
                console.error("Error stopping recognition: " + err);
            }
        );
    }
}