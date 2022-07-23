$(document).ready(function () {

    $(".custom-delete-button").click(function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'A jeni te sigurte?',
            text: "Perdoruesi i fshire nuk mund te rikthehet!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Po, fshije!'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'U fshi me sukses!',
                    text: 'Perdoruesi ne fjale u fshi me sukses.',
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 1000
                }).then((result) => {
                    /*if (result.isConfirmed) {*/
                    this.parentElement.submit();
                    /*}*/
                })
            }
        })
    })

})
