window.saveFile = async (contentStreamReference, fileName, fileExtension) => {
    if (!'showSaveFilePicker' in window) return await downloadFileFromStream(fileName, contentStreamReference);

    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);

    const options = {
        suggestedName: fileName,
        types: [{
            description: '',
            accept: {'text/plain': fileExtension},
        }],
    };

    try {

        const fileHandle = await window.showSaveFilePicker(options);
        const writableStream = await fileHandle.createWritable();
        await writableStream.write(blob);
        await writableStream.close();
    } catch (e) {
        if (e.name === "AbortError") {
            
        } else {
            throw e;
        }
    }
}

async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function triggerFileDownload(fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}