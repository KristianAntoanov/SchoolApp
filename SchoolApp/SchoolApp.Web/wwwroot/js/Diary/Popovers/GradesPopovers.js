
$(document).ready(function () {
    function initializePopovers() {
        $('[data-bs-toggle="popover"]').popover({
            trigger: 'click',
            html: true,
            container: 'body'
        });
    }

    initializePopovers();

    $(document).on('click', '[data-bs-toggle="popover"]', function (e) {

        const $this = $(this);
        const isVisible = $this.data('bs.popover') &&
            $this.data('bs.popover').tip &&
            $this.data('bs.popover').tip.classList.contains('show');

        $('[data-bs-toggle="popover"]').not(this).popover('hide');

        if (isVisible) {
            $this.popover('hide');
        } else {
            $this.popover('show');
        }
    });

    $(document).on('click', function (e) {
        if ($(e.target).data('bs-toggle') !== 'popover' &&
            $(e.target).parents('[data-bs-toggle="popover"]').length === 0 &&
            $(e.target).parents('.popover').length === 0) {

            $('[data-bs-toggle="popover"]').popover('hide');
        }
    });

    $(document).on('hidden.bs.popover', function (e) {
        $(e.target).data('bs-popover', null);
        initializePopovers();
    });
});