// Global objects
var SpeechSDK;
var avatarSynthesizer;
var peerConnection;
var previousAnimationFrameTimestamp = 0;
var remoteVideoDiv;
var canvas;
var transparentBackground = false;
var mediaPlayer;

export function initializeAvatar() {
    if (!!window.SpeechSDK) {
        SpeechSDK = window.SpeechSDK;
        console.log("SpeechSDK loaded");
    }

    if (window.innerHeight < 1300) {
        var videoContainer = document.getElementById('videoContainer');
        var overlayImage = document.getElementById('overlayImage');
        var canvas = document.getElementById('canvas');
        var tmpCanvas = document.getElementById('tmpCanvas');

        videoContainer.style.height = '980px';
        overlayImage.style.height = '980px';
        canvas.style.height = '980px';
        tmpCanvas.style.height = '980px';
    }

}

// Setup WebRTC
export function setupWebRTC(iceServerUrl, iceServerUsername, iceServerCredential) {
    // Create WebRTC peer connection
    peerConnection = new RTCPeerConnection({
        iceServers: [
            {
                urls: [iceServerUrl],
                username: iceServerUsername,
                credential: iceServerCredential
            }
        ]
    });

    // Fetch WebRTC video stream and mount it to an HTML video element
    peerConnection.ontrack = function (event) {
        // Clean up existing video element if there is any
        remoteVideoDiv = document.getElementById('remoteVideo');
        for (var i = 0; i < remoteVideoDiv.childNodes.length; i++) {
            if (remoteVideoDiv.childNodes[i].localName === event.track.kind) {
                remoteVideoDiv.removeChild(remoteVideoDiv.childNodes[i]);
            }
        }

        mediaPlayer = document.createElement(event.track.kind);
        mediaPlayer.id = event.track.kind;
        mediaPlayer.srcObject = event.streams[0];
        mediaPlayer.autoplay = true;
        document.getElementById('remoteVideo').appendChild(mediaPlayer);

        if (event.track.kind === 'video') {
            mediaPlayer.playsInline = true;
            remoteVideoDiv = document.getElementById('remoteVideo');
            canvas = document.getElementById('canvas');
            if (transparentBackground == true) {
                remoteVideoDiv.style.width = '0.1px';
                canvas.getContext('2d').clearRect(0, 0, canvas.width, canvas.height);
                canvas.hidden = false;
            } else {
                canvas.hidden = true;
            }

            mediaPlayer.addEventListener('play',
                () => {
                    if (transparentBackground == true) {
                        window.requestAnimationFrame(makeBackgroundTransparent);
                    } else {
                        remoteVideoDiv.style.width = mediaPlayer.videoWidth / 2 + 'px';
                    }
                    document.getElementById('overlayArea').hidden = true;
                });
        }
        else {
            // Mute the audio player to make sure it can auto play, will unmute it when speaking
            // Refer to https://developer.mozilla.org/en-US/docs/Web/Media/Autoplay_guide
            mediaPlayer.muted = true;
        }
    }

    // Listen to data channel, to get the event from the server
    peerConnection.addEventListener("datachannel",
        event => {
            const dataChannel = event.channel;
            dataChannel.onmessage = e => {
                console.log("[" + (new Date()).toISOString() + "] WebRTC event received: " + e.data);
            }
        });

    // This is a workaround to make sure the data channel listening is working by creating a data channel from the client side
    var c = peerConnection.createDataChannel("eventChannel");

    // Make necessary update to the web page when the connection state changes
    peerConnection.oniceconnectionstatechange = e => {
        console.log("WebRTC status: " + peerConnection.iceConnectionState);

        if (peerConnection.iceConnectionState === 'connected') {
        }

        if (peerConnection.iceConnectionState === 'disconnected' || peerConnection.iceConnectionState === 'failed') {
        }
    }

    // Offer to receive 1 audio, and 1 video track
    peerConnection.addTransceiver('video', { direction: 'sendrecv' });
    peerConnection.addTransceiver('audio', { direction: 'sendrecv' });

    // start avatar, establish WebRTC connection
    avatarSynthesizer.startAvatarAsync(peerConnection).then((r) => {
        if (r.reason === SpeechSDK.ResultReason.SynthesizingAudioCompleted) {
            console.log("[" + (new Date()).toISOString() + "] Avatar started. Result ID: " + r.resultId);
        } else {
            console.log("[" + (new Date()).toISOString() + "] Unable to start avatar. Result ID: " + r.resultId);
            if (r.reason === SpeechSDK.ResultReason.Canceled) {
                let cancellationDetails = SpeechSDK.CancellationDetails.fromResult(r);
                if (cancellationDetails.reason === SpeechSDK.CancellationReason.Error) {
                    console.log(cancellationDetails.errorDetails)
                };
                console.log("Unable to start avatar: " + cancellationDetails.errorDetails);
            }
        }
    }).catch(
        (error) => {
            console.log("[" + (new Date()).toISOString() + "] Avatar failed to start. Error: " + error);
        }
    );
}

// Make video background transparent by matting
export function makeBackgroundTransparent(timestamp) {
    // Throttle the frame rate to 30 FPS to reduce CPU usage
    if (timestamp - previousAnimationFrameTimestamp > 30) {
        var video = document.getElementById('video');
        var tmpCanvas = document.getElementById('tmpCanvas');
        var tmpCanvasContext = tmpCanvas.getContext('2d', { willReadFrequently: true });
        tmpCanvasContext.drawImage(video, 0, 0, video.videoWidth, video.videoHeight);
        if (video.videoWidth > 0) {
            let frame = tmpCanvasContext.getImageData(0, 0, video.videoWidth, video.videoHeight);
            for (let i = 0; i < frame.data.length / 4; i++) {
                let r = frame.data[i * 4 + 0];
                let g = frame.data[i * 4 + 1];
                let b = frame.data[i * 4 + 2];
                if (g - 150 > r + b) {
                    // Set alpha to 0 for pixels that are close to green
                    frame.data[i * 4 + 3] = 0;
                } else if (g + g > r + b) {
                    // Reduce green part of the green pixels to avoid green edge issue
                    var adjustment = (g - (r + b) / 2) / 3;
                    r += adjustment;
                    g -= adjustment * 2;
                    b += adjustment;
                    frame.data[i * 4 + 0] = r;
                    frame.data[i * 4 + 1] = g;
                    frame.data[i * 4 + 2] = b;
                    // Reduce alpha part for green pixels to make the edge smoother
                    var a = Math.max(0, 255 - adjustment * 4);
                    frame.data[i * 4 + 3] = a;
                }
            }

            canvas = document.getElementById('canvas');
            var canvasContext = canvas.getContext('2d');
            canvasContext.putImageData(frame, 0, 0);
        }

        previousAnimationFrameTimestamp = timestamp;
    }

    window.requestAnimationFrame(makeBackgroundTransparent);
}
export function startSession(dotNetHelper, language, voice) {
    const cogSvcRegion = "swedencentral";
    const cogSvcSubKey = "e3bb29e86fcd4aebaf96684a6893bcea";

    let speechSynthesisConfig = SpeechSDK.SpeechConfig.fromSubscription(cogSvcSubKey, cogSvcRegion);
    speechSynthesisConfig.speechSynthesisLanguage = language;
    speechSynthesisConfig.speechSynthesisVoiceName = voice;   
    const videoFormat = new SpeechSDK.AvatarVideoFormat();
    let videoCropTopLeftX = 600;
    let videoCropBottomRightX = 1320;
    let videoCropBottomRightY = window.innerHeight < 1300 ? 980 : 1080;
    videoFormat.setCropRange(new SpeechSDK.Coordinate(videoCropTopLeftX, 0), new SpeechSDK.Coordinate(videoCropBottomRightX, videoCropBottomRightY));


    const talkingAvatarCharacter = "lisa";
    const talkingAvatarStyle = "casual-sitting";
    const avatarConfig = new SpeechSDK.AvatarConfig(talkingAvatarCharacter, talkingAvatarStyle, videoFormat);
    //avatarConfig.customized = document.getElementById('customizedAvatar').checked;
    //avatarConfig.backgroundColor = document.getElementById('backgroundColor').value;
    avatarSynthesizer = new SpeechSDK.AvatarSynthesizer(speechSynthesisConfig, avatarConfig);
    avatarSynthesizer.avatarEventReceived = function (s, e) {
        var offsetMessage = ", offset from session start: " + e.offset / 10000 + "ms.";
        if (e.offset === 0) {
            offsetMessage = "";
        }
        console.log("[" + (new Date()).toISOString() + "] Event received: " + e.description + offsetMessage);
    }

    //document.getElementById('startSession').disabled = true

    const xhr = new XMLHttpRequest();
    xhr.open("GET", `https://${cogSvcRegion}.tts.speech.microsoft.com/cognitiveservices/avatar/relay/token/v1`);
    xhr.setRequestHeader("Ocp-Apim-Subscription-Key", cogSvcSubKey);
    xhr.addEventListener("readystatechange",
        function() {
            if (this.readyState === 4) {
                const responseData = JSON.parse(this.responseText);
                const iceServerUrl = responseData.Urls[0];
                const iceServerUsername = responseData.Username;
                const iceServerCredential = responseData.Password;
                setupWebRTC(iceServerUrl, iceServerUsername, iceServerCredential);
            }
        });
    xhr.send();
}
export function stopSession() {
    avatarSynthesizer.close();
}
export function speak(textToSpeak) {
    try {
        mediaPlayer.muted = false;
        avatarSynthesizer.speakTextAsync(textToSpeak).then(
            (result) => {
                if (result.reason === SpeechSDK.ResultReason.SynthesizingAudioCompleted) {
                    console.log("Speech and avatar synthesized to video stream.");
                } else {
                    console.log("Unable to speak. Result ID: " + result.resultId);
                    if (result.reason === SpeechSDK.ResultReason.Canceled) {
                        let cancellationDetails = SpeechSDK.CancellationDetails.fromResult(result);
                        console.log(cancellationDetails.reason);
                        if (cancellationDetails.reason === SpeechSDK.CancellationReason.Error) {
                            console.log(cancellationDetails.errorDetails);
                        }
                    }
                }
            }).catch((error) => {
                console.log(error);
                avatarSynthesizer.close();
            });
    }
    catch (error) {
        console.log(error);
        avatarSynthesizer.close();
    }
}